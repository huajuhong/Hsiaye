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
using Dapper;

namespace Hsiaye.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RoleController : ControllerBase
    {
        private readonly IDatabase _database;
        private readonly IAccessor _accessor;
        //private readonly IRoleService _roleService;
        private readonly IPermissionChecker _permissionChecker;

        public RoleController(IDatabase database, IAccessor accessor, IPermissionChecker permissionChecker)
        {
            _database = database;
            _accessor = accessor;
            _permissionChecker = permissionChecker;
        }

        //public IEnumerable<Role> Test()
        //{
        //    var list = _database.Connection.Query<Role>("Select * From Role where OrganizationUnitId.IsDescendantOf('/1/2/')=1");
        //    //var list = _database.Connection.Query<Role>("Select * From Role where OrganizationUnitId.IsDescendantOf(@OrganizationUnitId)=1", new { OrganizationUnitId = "/1/2/" });
        //    return list;
        //}

        /// <summary>
        /// 当前用户的所有角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Role> Current()
        {
            var roleIds = _database.GetList<MemberRole>(Predicates.Field<MemberRole>(f => f.MemberId, Operator.Eq, _accessor.MemberId)).Select(r => r.RoleId);

            var list = _database.GetList<Role>(Predicates.Field<Role>(e => e.Id, Operator.Eq, roleIds));

            foreach (var model in list)
            {
                MapToEntity(model);
            }

            return list;
        }

        [HttpPost]
        [Authorize(PermissionNames.角色_新建)]
        public bool Create(CreateRoleDto input)
        {
            List<IPredicate> predicates = new List<IPredicate>
            {
                Predicates.Field<Role>(f => f.Name, Operator.Eq, input.Name),
            };
            int count = _database.Count<Role>(Predicates.Group(GroupOperator.And, predicates.ToArray()));
            if (count > 0)
            {
                throw new UserFriendlyException($"已存在角色：{input.Name}");
            }
            var entity = new Role
            {
                CreateTime = DateTime.Now,
                CreatorId = _accessor.MemberId,
                Description = input.Description,
                DisplayName = input.DisplayName,
                IsDefault = false,
                IsStatic = false,
                Name = input.Name,
            };
            try
            {
                _database.BeginTransaction();
                int id = _database.Insert(entity);
                var permissions = new List<Permission>();
                foreach (var permission in input.GrantedPermissions)
                {
                    if (!_permissionChecker.IsGranted(permission))
                        throw new UserFriendlyException($"无权限操作权限：{permission}");
                    permissions.Add(new Permission
                    {
                        CreatorMemberId = _accessor.MemberId,
                        IsGranted = true,
                        MemberId = 0,
                        RoleId = id,
                        Name = permission,
                    });
                }
                _database.Insert(permissions.AsEnumerable());
                _database.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _database.Rollback();
                throw new UserFriendlyException(ex);
            }
        }

        [HttpPost]
        [Authorize(PermissionNames.角色_列表)]
        public PageResult<Role> List(KeywordsListInput input)
        {
            IPredicateGroup predicateGroup = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            if (!string.IsNullOrEmpty(input.Keywords))
            {
                predicateGroup.Predicates.Add(Predicates.Field<Role>(e => e.Name, Operator.Like, input.Keywords));
                predicateGroup.Predicates.Add(Predicates.Field<Role>(e => e.DisplayName, Operator.Like, input.Keywords));
                predicateGroup.Predicates.Add(Predicates.Field<Role>(e => e.Description, Operator.Like, input.Keywords));
            }
            var sort = new List<ISort> { Predicates.Sort<Role>(x => x.CreateTime) };
            var list = _database.GetPage<Role>(predicateGroup, sort, input.PageIndex, input.PageSize).ToList();

            foreach (var model in list)
            {
                MapToEntity(model);
            }

            var count = _database.Count<Role>(predicateGroup);
            return new PageResult<Role>(list, count);
        }

        [HttpGet]
        [Authorize(PermissionNames.角色_详情)]
        public Role Get(int id)
        {
            var model = _database.Get<Role>(id);
            MapToEntity(model);
            return model;
        }

        [HttpPost]
        [Authorize(PermissionNames.角色_编辑)]
        public bool Update(Role input)
        {
            var role = _database.Get<Role>(input.Id);
            role.Name = input.Name;
            role.DisplayName = input.DisplayName;
            role.Description = input.Description;
            //if (!_accessor.RoleIds.Any(rid => rid == role.Id))
            //    throw new UserFriendlyException($"无权限操作角色：{role.Name}");
            foreach (var item in input.Permissions)
            {
                // todo:待解除
                //if (!PermissionNames.Permissions.Exists(x => x.Name == item))
                //{
                //    throw new UserFriendlyException($"系统内置权限中没有[{item}]");
                //}
            }

            try
            {
                _database.BeginTransaction();
                _database.Update(role);
                var predicate = Predicates.Field<Permission>(f => f.RoleId, Operator.Eq, role.Id);
                var permissions = _database.GetList<Permission>(predicate).ToList();
                if (permissions.Any())
                    _database.Delete<Permission>(predicate);
                permissions = new List<Permission>();
                foreach (var permissionName in input.Permissions)
                {
                    permissions.Add(new Permission
                    {
                        CreatorMemberId = _accessor.MemberId,
                        IsGranted = true,
                        //Name = permissionName,// todo:待解除
                        RoleId = role.Id,
                        MemberId = 0
                    });
                }
                _database.Insert(permissions.AsEnumerable());
                _database.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _database.Rollback();
                throw new UserFriendlyException(ex);
            }
            finally
            {
                _database.Dispose();
            }
        }

        private void MapToEntity(Role model)
        {
            var permissions = _database.GetList<Permission>(Predicates.Field<Permission>(f => f.RoleId, Operator.Eq, model.Id));
            model.Permissions = permissions;
        }
        //[HttpGet]
        //[Authorize(PermissionNames.角色_编辑)]
        //public bool Delete(int id)
        //{
        //    if (!_accessor.RoleIds.Contains(id))
        //        return false;
        //    var role = _database.Get<Role>(id);
        //    if (role.IsDefault)
        //        return false;
        //    if (role.IsStatic)
        //        return false;
        //    _database.Delete(role);
        //    return true;
        //}
    }
}