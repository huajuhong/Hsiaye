using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Domain
{
    public class Role
    {
        public int Id { get; set; }
        public long CreatorId { get; set; }//创建者id
        public DateTime CreateTime { get; set; }
        public string DisplayName { get; set; }//显示名称
        public bool IsDefault { get; set; }
        public bool IsStatic { get; set; }//是否内置
        public string Name { get; set; }//名称
        public int TenantId { get; set; }
        public string Description { get; set; }//描述
    }
}
