using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Application.Contracts.Role.Dto
{
    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }

        public List<PermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}
