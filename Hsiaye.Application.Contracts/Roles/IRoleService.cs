using Hsiaye.Application.Contracts.Roles.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Application.Contracts.Roles
{
    public interface IRoleService
    {
        RoleDto Create(CreateRoleDto input);

        RoleDto Update(RoleDto input);

        void Delete(int id);

        RoleDto Get(long id);

        List<RoleDto> GetAll(string keyword, int page, int limit);

        public List<PermissionDto> GetAllPermissions();

        public List<RoleDto> GetAll();

        List<RoleListDto> GetRoles(string permission);
    }
}
