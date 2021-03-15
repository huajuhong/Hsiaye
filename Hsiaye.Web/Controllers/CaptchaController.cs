using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Hsiaye.Web.Controllers
{
    //验证码生成
    public class CaptchaController : Controller
    {
        private static readonly Rgba32 red = new Rgba32(255, 87, 34);
        private static readonly List<Rgba32> rgba32sText = new List<Rgba32>
        {
            new Rgba32(255, 129, 61),
            new Rgba32(58, 215, 254),
            new Rgba32(38, 186, 255),
            new Rgba32(18, 203, 119),
            new Rgba32(254, 174, 58),
            new Rgba32(131, 128, 128),
        };
        private static readonly List<Rgba32> rgba32sBackground = new List<Rgba32>
        {
            new Rgba32(86, 163, 108),
            new Rgba32(94, 133, 121),
            new Rgba32(46, 104, 170),
            new Rgba32(126, 136, 79),
            new Rgba32(224, 128, 49),
            new Rgba32(25, 148, 117),
            new Rgba32(11, 110, 72),
            new Rgba32(4, 77, 34),
        };
        public IActionResult Get(string username)
        {
            string text = Guid.NewGuid().ToString("N").Substring(0, 4);

            return Json(text);
        }
    }
}
