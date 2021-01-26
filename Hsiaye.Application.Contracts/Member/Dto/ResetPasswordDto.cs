using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Application.Contracts.Member.Dto
{
    public class ResetPasswordDto
    {
        public string AdminPassword { get; set; }

        public long MemberId { get; set; }

        public string NewPassword { get; set; }
    }
}
