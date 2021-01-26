using Hsiaye.Application.Contracts.Role.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Application.Contracts.Role
{
    public interface IRoleService
    {
        RoleDto Create(CreateRoleDto input);

        RoleDto Update(RoleDto input);

        void Delete(int id);

        RoleDto Get(long id);
        RoleDto GetAll(string Keyword, bool IsActive, int SkipCount, int MaxResultCount);

        public List<PermissionDto> GetAllPermissions();

        GetRoleForEditOutput GetRoleForEdit(int id);

        List<RoleListDto> GetRoles(string permission);
    }
}
