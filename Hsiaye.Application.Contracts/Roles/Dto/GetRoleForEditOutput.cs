using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Application.Contracts
{
    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }

        /// <summary>
        /// 当前登陆者可配置的权限
        /// </summary>
        public List<PermissionDto> Permissions { get; set; }

        /// <summary>
        /// 当前角色拥有的权限名称
        /// </summary>
        public List<string> GrantedPermissionNames { get; set; }
    }
}
