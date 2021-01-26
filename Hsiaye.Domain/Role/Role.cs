using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Domain.Role
{
    public class Role
    {
        public string Name { get; set; }//名称
        public string DisplayName { get; set; }//显示名称
        public string Description { get; set; }//
        public bool IsStatic { get; set; }//是否内置
    }
}
