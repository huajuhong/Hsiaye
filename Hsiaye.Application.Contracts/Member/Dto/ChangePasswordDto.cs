using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Application.Contracts.Member.Dto
{
    public class ChangePasswordDto
    {
        //[Required]
        public string CurrentPassword { get; set; }

        //[Required]
        public string NewPassword { get; set; }
    }
}
