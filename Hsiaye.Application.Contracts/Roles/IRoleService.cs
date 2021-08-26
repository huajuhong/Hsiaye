using Hsiaye.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Application.Contracts
{
    public interface IRoleService
    {
        /// <summary>
        /// 拥有该权限的角色
        /// </summary>
        /// <param name="permission">权限名</param>
        /// <returns></returns>
        List<Role> GetList(string permission);
    }
}