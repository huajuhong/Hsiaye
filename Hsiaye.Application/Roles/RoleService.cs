using Hsiaye.Application.Contracts;
using Hsiaye.Dapper;
using Hsiaye.Domain;
using Hsiaye.Domain.Shared;
using Hsiaye.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool Create(CreateRoleDto input)
        {
            List<IPredicate> predicates = new List<IPredicate>
            {
                Predicates.Field<Role>(f => f.Name, Operator.Eq, input.Name),
                Predicates.Field<Role>(f => f.TenantId, Operator.Eq, _accessor.TenantId)
            };
            int count = _database.Count<Role>(Predicates.Group(GroupOperator.And, predicates.ToArray()));
            if (count > 0)
            {
                throw new UserFriendlyException($"已存在角色：{input.Name}");
            }
            var entity = new Role
            {
                Description = input.Description,
                DisplayName = input.DisplayName,
                IsDefault = false,
                IsStatic = false,
                Name = input.Name,
                TenantId = _accessor.TenantId,
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
                _database.Insert(permissions);
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
            if (!_accessor.RoleIds.Any(rid => rid == role.Id))
                throw new UserFriendlyException($"无权限操作角色：{role.Name}");
            _database.BeginTransaction();
            try
            {
                _database.Update(role);
                var permissions = _database.GetList<Permission>(Predicates.Field<Permission>(f => f.RoleId, Operator.Eq, role.Id)).ToList();
                if (permissions.Any())
                    _database.Delete(permissions);
                permissions = new List<Permission>();
                foreach (var permissionName in input.GrantedPermissions)
                {
                    permissions.Add(new Permission
                    {
                        CreatorMemberId = _accessor.MemberId,
                        IsGranted = true,
                        Name = permissionName,
                        TenantId = _accessor.TenantId,
                        RoleId = role.Id,
                        MemberId = 0
                    });
                }
                _database.Insert(permissions);
                _database.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _database.Rollback();
                throw new UserFriendlyException(ex);
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
            if (role.TenantId != _accessor.TenantId)
                return;
            _database.Delete(role);
        }

        public RoleDto Get(int id)
        {
            var role = _database.Get<Role>(id);
            var permissions = _database.GetList<Permission>(Predicates.Field<Permission>(f => f.RoleId, Operator.Eq, role.Id));
            var roleDto = ExpressionGenericMapper<Role, RoleDto>.MapperTo(role);
            roleDto.GrantedPermissions = permissions.ToList().FindAll(x => x.IsGranted).Select(x => x.Name).ToList();
            return roleDto;
        }

        public List<RoleDto> GetAll(string keyword, int page, int limit)
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

        public List<PermissionDto> GetAllPermissions()
        {
            var predicate = Predicates.Field<Member_Role>(f => f.MemberId, Operator.Eq, _accessor.MemberId);
            var member_Roles = _database.GetList<Member_Role>(predicate);

            List<int> roleIds = new List<int>();
            foreach (var item in member_Roles)
            {
                roleIds.Add(item.RoleId);
            }
            var permissions = _database.GetList<Permission>(Predicates.Field<Permission>(f => f.RoleId, Operator.Eq, roleIds));
            var permissionDtos = ExpressionGenericMapper<List<Permission>, List<PermissionDto>>.MapperTo(permissions.Distinct().ToList());
            return permissionDtos;
        }

        public GetRoleForEditOutput GetRoleForEdit(int id)
        {
            var role = _database.Get<Role>(id);
            var roleEditDto = ExpressionGenericMapper<Role, RoleEditDto>.MapperTo(role);

            var permissions = _database.GetList<Permission>(Predicates.Field<Permission>(f => f.RoleId, Operator.Eq, id));
            var permissionDtos = ExpressionGenericMapper<List<Permission>, List<PermissionDto>>.MapperTo(permissions.ToList());

            var output = new GetRoleForEditOutput
            {
                Role = roleEditDto,
                Permissions = permissionDtos,
                GrantedPermissionNames = permissions.Where(x => x.IsGranted).Select(p => p.Name).ToList(),
            };
            return output;
        }

        public List<RoleListDto> GetRoles(string permission)
        {
            var predicate = Predicates.Field<Member_Role>(f => f.MemberId, Operator.Eq, _accessor.MemberId);
            var member_Roles = _database.GetList<Member_Role>(predicate);
            List<RoleDto> roleDtos = new List<RoleDto>();
            RoleDto roleDto;
            foreach (var item in member_Roles)
            {
                roleDto = Get(item.RoleId);
                roleDtos.Add(roleDto);
            }
            var roles = roleDtos.FindAll(r => r.GrantedPermissions.Any(p => p == permission));
            var dtos = ExpressionGenericMapper<List<RoleDto>, List<RoleListDto>>.MapperTo(roles);
            return dtos;
        }
    }
}
