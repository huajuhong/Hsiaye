using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Application.Contracts
{
    public interface IRoleService
    {
        List<RoleDto> Current();
        bool Create(CreateRoleDto input);
        bool Update(RoleDto input);
        void Delete(int id);
        RoleDto Get(int id);


        /// <summary>
        /// 编辑角色时的角色数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        GetRoleForEditOutput GetForEdit(int id);

        /// <summary>
        /// 拥有该权限的角色
        /// </summary>
        /// <param name="permission">权限名</param>
        /// <returns></returns>
        List<RoleListDto> GetList(string permission);
    }
}