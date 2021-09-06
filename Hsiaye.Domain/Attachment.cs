using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    public class Attachment
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string TableName { get; set; }
        public long FieldId { get; set; }
        public string FieldName { get; set; }
        public string FileName { get; set; }
        public string PhysicalPath { get; set; }
        public string RelativePath { get; set; }
        public string Extension { get; set; }
        public long FileSize { get; set; }
        public string MD5 { get; set; }
    }
    public class AttachmentMap : ClassMapper<Attachment>
    {
        public AttachmentMap()
        {
            Map(f => f.Id).Key(KeyType.Guid);
            AutoMap();
        }
    }
}
