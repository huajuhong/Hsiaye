using DapperExtensions;
using Hsiaye.Domain;
using Hsiaye.Domain.Shared;
using Hsiaye.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Web.Controllers
{
    //文件上传
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AttachmentController : ControllerBase
    {
        private readonly IDatabase _database;
        private readonly IWebHostEnvironment _env;
        //存放文件夹
        private static readonly string FOLDER = "upload";

        public AttachmentController(IWebHostEnvironment env, IDatabase database)
        {
            _env = env;
            _database = database;
        }

        [HttpPost]
        public Attachment Upload()
        {
            if (Request.Form == null || Request.Form.Files == null || Request.Form.Files.Count < 1)
                throw new UserFriendlyException("请选择上传文件");
            IFormFile file = Request.Form.Files[0];
            if (file.Length < 1)
                throw new UserFriendlyException("文件无效");
            Guid fileId = Guid.NewGuid();
            string extension = Path.GetExtension(file.FileName);
            //新文件名
            var fileName = fileId + extension;

            //物理路径
            string physicalPath = Path.Combine(_env.WebRootPath, FOLDER);

            if (!Directory.Exists(physicalPath))
                Directory.CreateDirectory(physicalPath);

            string path = Path.Combine(physicalPath, fileName);

            using var stream = new FileStream(path, FileMode.CreateNew);
            file.CopyTo(stream);

            var attachment = new Attachment
            {
                Id = fileId,
                CreateTime = DateTime.Now,
                TableName = "",
                FieldId = 0,
                FieldName = "",
                FileName = fileName,
                PhysicalPath = path,
                RelativePath = "/" + FOLDER + "/" + fileName,
                Extension = extension,
                FileSize = file.Length / 1024,
                MD5 = "",
            };

            _database.Insert(attachment);

            return attachment;
        }
    }
}
