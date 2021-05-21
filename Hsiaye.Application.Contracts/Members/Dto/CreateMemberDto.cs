using Hsiaye.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hsiaye.Application.Contracts
{

    public class CreateMemberDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        public MemberState State { get; set; }

        [Required]
        public string[] RoleNames { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
