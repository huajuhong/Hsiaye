using Hsiaye.Application;
using Hsiaye.Application.Contracts;
using DapperExtensions;
using DapperExtensions.Predicate;
using Hsiaye.Domain;
using Hsiaye.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hsiaye.Domain.Shared;

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

        /// <summary>
        /// 当前用户的所有角色
        /// </summary>
        /// <returns></returns>
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
        //todo:列表待优化，分页问题
        [HttpPost]
        [Authorize(PermissionNames.角色_列表)]
        public PageResult<RoleDto> List(KeywordsListInput input)
        {
            IPredicateGroup predicate = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            if (!string.IsNullOrEmpty(input.Keywords))
            {
                predicate.Predicates.Add(Predicates.Field<Role>(e => e.Name, Operator.Like, input.Keywords));
                predicate.Predicates.Add(Predicates.Field<Role>(e => e.DisplayName, Operator.Like, input.Keywords));
                predicate.Predicates.Add(Predicates.Field<Role>(e => e.Description, Operator.Like, input.Keywords));
            }
            var sort = new List<ISort> { Predicates.Sort<Role>(x => x.CreateTime) };
            var list = _database.GetPage<Role>(Predicates.Group(GroupOperator.Or, predicate.Predicates.ToArray()), sort, input.PageIndex, input.PageSize).ToList();

            var listDto = ExpressionGenericMapper<Role, RoleDto>.MapperTo(list);
            if (listDto == null)
                return null;
            foreach (var roleDto in listDto)
            {
                var permissions = _database.GetList<Permission>(Predicates.Field<Permission>(f => f.RoleId, Operator.Eq, roleDto.Id));
                roleDto.GrantedPermissions = permissions.ToList().FindAll(x => x.IsGranted).Select(x => x.Name).ToList();
            }

            var count = _database.Count<Role>(predicate);
            return new PageResult<RoleDto>(listDto, count);
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
