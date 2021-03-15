using Hsiaye.Application.Contracts.Roles.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Application.Contracts.Roles
{
    public interface IRoleService
    {
        bool Create(CreateRoleDto input);
        bool Update(RoleDto input);
        void Delete(int id);
        RoleDto Get(int id);

        List<RoleDto> GetAll(string keyword, int page, int limit);

        /// <summary>
        /// 当前用户的角色拥有的权限
        /// </summary>
        /// <returns></returns>
        public List<PermissionDto> GetAllPermissions();

        /// <summary>
        /// 当前用户拥有的角色
        /// </summary>
        /// <returns></returns>

        public List<RoleDto> GetAll();

        /// <summary>
        /// 编辑角色时的角色数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        GetRoleForEditOutput GetRoleForEdit(int id);

        /// <summary>
        /// 拥有该权限的角色
        /// </summary>
        /// <param name="permission">权限名</param>
        /// <returns></returns>
        List<RoleListDto> GetRoles(string permission);
    }
}