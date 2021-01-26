using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Application.Contracts.Role.Dto
{
    public class CreateRoleDto
    {
        //[Required]
        public string Name { get; set; }

        //[Required]
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public List<string> GrantedPermissions { get; set; }

        public CreateRoleDto()
        {
            GrantedPermissions = new List<string>();
        }
    }
}
