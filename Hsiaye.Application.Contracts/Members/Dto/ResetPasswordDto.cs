using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Application.Contracts.Members.Dto
{
    public class ResetPasswordDto
    {
        public string AdminPassword { get; set; }

        public long MemberId { get; set; }

        public string NewPassword { get; set; }
    }
}
