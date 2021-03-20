﻿using Hsiaye.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Hsiaye.Web.Controllers
{
    //验证码生成
    [Route("[controller]/[action]")]
    public class CaptchaController : Controller
    {
        private readonly IMemoryCache _cache;

        public CaptchaController(IMemoryCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string code = new Random(Guid.NewGuid().GetHashCode()).Next(0, 9999).ToString().PadLeft(4, '0');
            byte[] fileContents = GetVerifyCode(code);
            var image = "data:image/png;base64," + Convert.ToBase64String(fileContents);

            string key = "CaptchaImage" + Guid.NewGuid().ToString("N");
            _cache.Set(key, code, new TimeSpan(0, 3, 0));
            return Json(new { key, image });
            //return File(fileContents, "image/png");
        }
        private static byte[] GetVerifyCode(string code)
        {
            int width = 70;
            int height = 36;
            var v00 = new PointF(0, height);
            var v11 = new PointF(width, 0);
            Random random = new Random(Guid.NewGuid().GetHashCode());
            List<List<PointF>> pointFss = new List<List<PointF>>
            {
                new List<PointF> { v00, new PointF(random.Next(width / 2, width) * 1.3F, 0), new PointF(random.Next(0, width / 2), height * 1.3F), v11 },
                new List<PointF> { v00, new PointF(random.Next(0, width), height * 1.3F), new PointF(random.Next(0, width) * 1.3F, 0), v11 },
            };
            const int fontSize = 20;
            Font ft = new Font("consolas", fontSize);
            //Font ft = new Font("SimHei", fontSize); 
            //颜色列表，用于验证码、噪线、噪点 
            Color[] color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };

            //创建画布
            using Bitmap bmp = new Bitmap(width, height);
            using Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            Color clr = color[random.Next(color.Length)];
            //画噪线 
            for (int i = 0; i < 3; i++)
            {
                int x1 = random.Next(width);
                int y1 = random.Next(height);
                int x2 = random.Next(width);
                int y2 = random.Next(height);
                g.DrawLine(new Pen(clr), x1, y1, x2, y2);
                g.DrawBeziers(new Pen(clr, 2), pointFss[random.Next(0, 1)].ToArray());
            }
            //画验证码字符串 
            for (int i = 0; i < code.Length; i++)
            {
                g.DrawString(code[i].ToString(), ft, new SolidBrush(clr), (float)i * 12 + 5, 2);
            }
            //将验证码图片写入内存流，并将其以 "image/Png" 格式输出 
            using MemoryStream ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        [HttpGet]
        public IActionResult Verify(string key, string code)
        {
            string value = _cache.Get<string>(key);
            if (string.IsNullOrEmpty(value))
                throw new UserFriendlyException("图形验证码已过期");
            if (!value.Equals(code, StringComparison.OrdinalIgnoreCase))
                throw new UserFriendlyException("图形验证码错误");
            //业务代码
            _cache.Remove(key);
            return Ok();
        }
    }
}