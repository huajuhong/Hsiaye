using Hsiaye.Application.Contracts.Authorization;
using Hsiaye.Application.Contracts.Roles;
using Hsiaye.Application.Contracts.Roles.Dto;
using Hsiaye.Dapper;
using Hsiaye.Domain.Authorization;
using Hsiaye.Domain.Members;
using Hsiaye.Domain.Roles;
using Hsiaye.Domain.Shared;
using Hsiaye.Extensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Roles
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

        public RoleDto Create(CreateRoleDto input)
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
                        throw new UserFriendlyException($"无法操作权限：{permission}");
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
                return dto;
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

        public RoleDto Get(long id)
        {
            var role = _database.Get<Role>(id);
            var permissions = _database.GetList<Permission>(Predicates.Field<Permission>(f => f.RoleId, Operator.Eq, role.Id));
            var roleDto = ExpressionGenericMapper<Role, RoleDto>.MapperTo(role);
            if (permissions != null && permissions.Count() > 0)
                roleDto.GrantedPermissions = permissions.ToList().FindAll(x => x.IsGranted).Select(x => x.Name).ToList();
            return roleDto;
        }

        public RoleDto GetAll(string Keyword, bool IsActive, int SkipCount, int MaxResultCount)
        {
            throw new NotImplementedException();
        }

        public List<RoleDto> GetAll()
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
            return roleDtos;
        }

        public List<PermissionDto> GetAllPermissions()
        {
            var predicate = Predicates.Field<Member_Role>(f => f.MemberId, Operator.Eq, _accessor.MemberId);
            var member_Roles = _database.GetList<Member_Role>(predicate);
            List<PermissionDto> permissionDtos = new List<PermissionDto>();
            foreach (var item in member_Roles)
            {
                var permissions = _database.GetList<Permission>(Predicates.Field<Permission>(f => f.RoleId, Operator.Eq, item.RoleId));
                var dtos = ExpressionGenericMapper<List<Permission>, List<PermissionDto>>.MapperTo(permissions.ToList());
                permissionDtos.AddRange(dtos);
            }
            return permissionDtos;
        }

        public GetRoleForEditOutput GetRoleForEdit(long id)
        {
            throw new NotImplementedException();
        }

        public List<RoleListDto> GetRoles(string permission)
        {
            var roles = GetAll().FindAll(r => r.GrantedPermissions.Any(p => p == permission));
            var dtos = ExpressionGenericMapper<List<RoleDto>, List<RoleListDto>>.MapperTo(roles);
            return dtos;
        }


        public RoleDto Update(RoleDto input)
        {
            throw new NotImplementedException();
        }
    }
}
