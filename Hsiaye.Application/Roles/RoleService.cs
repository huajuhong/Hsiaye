using Hsiaye.Application.Contracts;
using DapperExtensions;
using Hsiaye.Domain;
using Hsiaye.Domain.Shared;
using Hsiaye.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Predicate;

namespace Hsiaye.Application
{
    public class RoleService : IRoleService
    {
        private readonly IDatabase _database;
        private readonly IAccessor _accessor;
        private readonly IPermissionChecker _permissionChecker;

        public RoleService(IDatabase database, IAccessor accessor, IPermissionChecker permissionChecker)
        {
            _database = database;
            _accessor = accessor;
            _permissionChecker = permissionChecker;
        }

        public List<RoleDto> Current()
        {
            var roleIds = _database.GetList<MemberRole>(Predicates.Field<MemberRole>(f => f.MemberId, Operator.Eq, _accessor.MemberId)).Select(r => r.RoleId);
            var result = new List<RoleDto>();
            foreach (var roleId in roleIds)
            {
                result.Add(Get(roleId));
            }
            return result;
        }

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
                var dto = ExpressionGenericMapper<CreateRoleDto, RoleDto>.MapperTo(input);
                dto.Id = id;
                return true;
            }
            catch (Exception ex)
            {
                _database.Rollback();
                throw new UserFriendlyException(ex);
            }
        }

        public bool Update(RoleDto input)
        {
            var role = _database.Get<Role>(input.Id);
            role.Name = input.Name;
            role.DisplayName = input.DisplayName;
            role.Description = input.Description;
            //if (!_accessor.RoleIds.Any(rid => rid == role.Id))
            //    throw new UserFriendlyException($"无权限操作角色：{role.Name}");
            foreach (var item in input.GrantedPermissions)
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
                foreach (var permissionName in input.GrantedPermissions)
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

        public void Delete(int id)
        {
            if (!_accessor.RoleIds.Contains(id))
                return;
            var role = _database.Get<Role>(id);
            if (role.IsDefault)
                return;
            if (role.IsStatic)
                return;
            _database.Delete(role);
        }

        public RoleDto Get(int id)
        {
            var role = _database.Get<Role>(id);
            if (role == null)
                throw new UserFriendlyException("该记录不存在");
            var permissions = _database.GetList<Permission>(Predicates.Field<Permission>(f => f.RoleId, Operator.Eq, role.Id));
            var roleDto = ExpressionGenericMapper<Role, RoleDto>.MapperTo(role);
            //roleDto.GrantedPermissions = permissions.ToList().FindAll(x => x.IsGranted).Select(x => x.Name).ToList();
            return roleDto;
        }

        public GetRoleForEditOutput GetForEdit(int id)
        {
            var role = _database.Get<Role>(id);
            var roleEditDto = ExpressionGenericMapper<Role, RoleEditDto>.MapperTo(role);

            var permissions = _database.GetList<Permission>(Predicates.Field<Permission>(f => f.RoleId, Operator.Eq, id));

            var output = new GetRoleForEditOutput
            {
                Role = roleEditDto,
                Permissions = ExpressionGenericMapper<Permission, PermissionDto>.MapperTo(_accessor.Permissions.ToList()),
                GrantedPermissionNames = permissions.Where(x => x.IsGranted).Select(p => p.Name).ToList(),
            };
            return output;
        }

        public List<RoleListDto> GetList(string permission)
        {
            var permissions = _database.GetList<Permission>(Predicates.Field<Permission>(f => f.Name, Operator.Eq, permission));
            if (!permissions.Any())
            {
                return null;
            }
            var roles = _database.GetList<Role>(Predicates.Field<Role>(f => f.Id, Operator.Eq, permissions.Select(x => x.RoleId))).ToList();
            var dtos = ExpressionGenericMapper<Role, RoleListDto>.MapperTo(roles);
            return dtos;
        }
    }
}
