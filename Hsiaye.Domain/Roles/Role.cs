using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hsiaye.Domain
{
    public class Role
    {
        public int Id { get; set; }
        public long CreatorId { get; set; }//创建者id
        public DateTime CreateTime { get; set; }
        [StringLength(64)]
        public string DisplayName { get; set; }//显示名称
        public bool IsDefault { get; set; }
        public bool IsStatic { get; set; }//是否是静态角色，静态角色为系统内置不可删除，只可修改显示名称
        [StringLength(64)]
        public string Name { get; set; }//名称
        [StringLength(1024)]
        public string Description { get; set; }//描述
    }
}
