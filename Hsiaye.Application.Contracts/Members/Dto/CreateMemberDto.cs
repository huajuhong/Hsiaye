using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Application.Contracts.Members.Dto
{
    
    public class CreateMemberDto
    {
        public string UserName { get; set; }

        public string Name { get; set; }

        //[Required]
        public string Surname { get; set; }

        //[Required]
        //[EmailAddress]
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public string[] RoleNames { get; set; }

        //[Required]
        public string Password { get; set; }

        public void Normalize()
        {
            if (RoleNames == null)
            {
                RoleNames = new string[0];
            }
        }
    }
}
