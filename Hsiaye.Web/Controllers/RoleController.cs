using Hsiaye.Application;
using Hsiaye.Application.Contracts;
using Hsiaye.Dapper;
using Hsiaye.Domain;
using Hsiaye.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hsiaye.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RoleController : ControllerBase
    {
        private readonly IDatabase _database;
        private readonly IRoleService _roleService;

        public RoleController(IDatabase database, IRoleService roleService)
        {
            _database = database;
            _roleService = roleService;
        }

        //当前用户的角色列表
        [HttpGet]
        public List<RoleDto> Current()
        {
            return _roleService.Current();
        }

        [HttpPost]
        [Authorize(PermissionNames.角色_新建)]
        public bool Create(CreateRoleDto input)
        {
            return _roleService.Create(input);
        }

        [HttpGet]
        [Authorize(PermissionNames.角色_列表)]
        public List<RoleDto> GetPage(string keyword, int page, int limit)
        {
            var predicates = new List<IPredicate>
            {
                Predicates.Field<Role>(e => e.Name, Operator.Like, keyword),
                Predicates.Field<Role>(e => e.DisplayName, Operator.Like, keyword),
                Predicates.Field<Role>(e => e.Description, Operator.Like, keyword),
            };
            var sort = new List<ISort> { Predicates.Sort<Role>(x => x.CreateTime) };
            var roles = _database.GetPage<Role>(Predicates.Group(GroupOperator.And, predicates.ToArray()), sort, page, limit).ToList();
            var roleDtos = ExpressionGenericMapper<Role, RoleDto>.MapperTo(roles);
            foreach (var roleDto in roleDtos)
            {
                var permissions = _database.GetList<Permission>(Predicates.Field<Permission>(f => f.RoleId, Operator.Eq, roleDto.Id));
                roleDto.GrantedPermissions = permissions.ToList().FindAll(x => x.IsGranted).Select(x => x.Name).ToList();
            }
            return roleDtos;
        }

        [HttpGet]
        [Authorize(PermissionNames.角色_详情)]
        public RoleDto Get(int id)
        {
            return _roleService.Get(id);
        }

        [HttpGet]
        [Authorize(PermissionNames.角色_编辑)]
        public GetRoleForEditOutput GetForEdit(int id)
        {
            return _roleService.GetForEdit(id);
        }

        [HttpPost]
        [Authorize(PermissionNames.角色_编辑)]
        public bool Update(RoleDto input)
        {
            return _roleService.Update(input);
        }
    }
}
