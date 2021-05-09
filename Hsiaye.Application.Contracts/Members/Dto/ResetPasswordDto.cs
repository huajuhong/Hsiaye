using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Application.Contracts
{
    public class ResetPasswordDto
    {
        public string AdminPassword { get; set; }

        public int MemberId { get; set; }

        public string NewPassword { get; set; }
    }
}
