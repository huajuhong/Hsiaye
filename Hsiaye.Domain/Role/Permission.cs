using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Domain.Permission
{
    public class Permission
    {
        public int ParentId { get; }
        public long CreatorMemberId { get; set; }
        public bool IsGranted { get; set; }
        public string Name { get; set; }
        public int RoleId { get; set; }
        public long MemberId { get; set; }
    }
}
