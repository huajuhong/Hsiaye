using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Domain.Member
{
    public class Member_Role
    {
        public long CreatorMemberId { get; set; }
        public int RoleId { get; set; }
        public long MemberId { get; set; }
    }
}
