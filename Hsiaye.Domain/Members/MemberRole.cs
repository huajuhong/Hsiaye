using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Domain
{
    public class MemberRole
    {
        public long Id { get; set; }
        public long CreatorMemberId { get; set; }
        public long RoleId { get; set; }
        public long MemberId { get; set; }
    }
}
