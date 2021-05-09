using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Domain
{
    public class MemberRole
    {
        public int Id { get; set; }
        public long CreatorMemberId { get; set; }
        public int RoleId { get; set; }
        public long MemberId { get; set; }
    }
}
