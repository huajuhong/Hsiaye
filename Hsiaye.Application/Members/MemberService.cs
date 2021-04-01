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
    public class MemberService : IMemberService
    {
        internal static readonly string AdminUserName = "hsiaye";
        private readonly IDatabase _database;
        private readonly IAccessor _accessor;

        public MemberService(IDatabase database, IAccessor accessor)
        {
            _database = database;
            _accessor = accessor;
        }

        public MemberDto Create(CreateMemberDto input)
        {
            _accessor.RoleAuthorize(input.RoleNames);
            Member model = new Member
            {
                CreateTime = DateTime.Now,
                UserName = input.UserName,
                Name = input.Name,
                EmailAddress = input.EmailAddress,
                IsActive = input.IsActive,
                Password = DESHelper.EncryptByGeneric(input.Password),
                EmailConfirmationCode = string.Empty,
                PasswordResetCode = string.Empty,
                PhoneNumber = string.Empty,
                TenantId = _accessor.TenantId,
            };
            try
            {
                _database.BeginTransaction();
                long id = _database.Insert(model);
                List<IPredicate> predicates = new List<IPredicate>
                {
                    Predicates.Field<Role>(f => f.Name, Operator.Eq, input.RoleNames),
                    Predicates.Field<Role>(f => f.TenantId, Operator.Eq, _accessor.TenantId)
                };

                var createRoles = _database.GetList<Role>(Predicates.Group(GroupOperator.And, predicates.ToArray()));

                if (createRoles != null)
                {
                    List<Member_Role> member_Roles = new List<Member_Role>();
                    foreach (var createRole in createRoles)
                    {
                        member_Roles.Add(new Member_Role
                        {
                            CreatorMemberId = _accessor.MemberId,
                            MemberId = id,
                            RoleId = createRole.Id,
                        });
                    }
                    _database.Insert(member_Roles);
                }
                _database.Commit();
            }
            catch (Exception ex)
            {
                _database.Rollback();
                throw new UserFriendlyException(ex);
            }
            var memberDto = ExpressionGenericMapper<CreateMemberDto, MemberDto>.MapperTo(input);
            memberDto.CreateTime = model.CreateTime;
            memberDto.LastLoginTime = model.LastLoginTime;
            return memberDto;
        }

        public MemberDto Update(MemberDto input)
        {
            _accessor.RoleAuthorize(input.RoleNames);

            var model = _database.Get<Member>(input.Id);
            model.UserName = input.UserName;
            model.Name = input.Name;
            model.EmailAddress = input.EmailAddress;
            model.IsActive = input.IsActive;
            try
            {
                _database.BeginTransaction();
                _database.Update(model);
                List<Member_Role> member_Roles = _database.GetList<Member_Role>(Predicates.Field<Member_Role>(f => f.MemberId, Operator.Eq, input.Id)).ToList();
                _database.Delete(member_Roles);
                member_Roles = new List<Member_Role>();

                var predicate = new List<IPredicate>();
                predicate.Add(Predicates.Field<Role>(f => f.Name, Operator.Eq, input.RoleNames));
                predicate.Add(Predicates.Field<Role>(f => f.TenantId, Operator.Eq, _accessor.TenantId));
                var roles = _database.GetList<Role>(predicate);
                foreach (var role in roles)
                {
                    member_Roles.Add(new Member_Role
                    {
                        CreatorMemberId = _accessor.MemberId,
                        MemberId = input.Id,
                        RoleId = role.Id,
                    });
                }
                _database.Insert(member_Roles);
                _database.Commit();
            }
            catch
            {
                _database.Rollback();
            }
            var memberDto = ExpressionGenericMapper<Member, MemberDto>.MapperTo(model);
            memberDto.RoleNames = input.RoleNames;
            return memberDto;
        }

        public void Activate(long id)
        {
            var model = _database.Get<Member>(id);
            if (model == null)
                return;
            if (model.IsActive)
                return;
            model.IsActive = true;
            _database.Update(model);
        }

        public void DeActivate(long id)
        {
            var model = _database.Get<Member>(id);
            if (model == null)
                return;
            if (!model.IsActive)
                return;
            model.IsActive = false;
            _database.Update(model);
        }

        public void Delete(long id)
        {
            var model = _database.Get<Member>(id);
            _database.Delete(model);
        }

        public MemberDto Get(long id)
        {
            var model = _database.Get<Member>(id);
            var memberDto = ExpressionGenericMapper<Member, MemberDto>.MapperTo(model);
            var roleIds = _database.GetList<Member_Role>(Predicates.Field<Member_Role>(f => f.MemberId, Operator.Eq, id)).Select(r => r.RoleId);
            if (roleIds.Any())
                memberDto.RoleNames = _database.GetList<Role>(Predicates.Field<Role>(f => f.Id, Operator.Eq, roleIds)).Select(x => x.Name).ToArray();
            return memberDto;
        }

        public bool ChangePassword(ChangePasswordDto input)
        {
            var model = _database.Get<Member>(_accessor.MemberId);
            if (model.Password != DESHelper.EncryptByGeneric(input.CurrentPassword))
            {
                throw new UserFriendlyException("当前密码不正确");
            }
            model.Password = DESHelper.EncryptByGeneric(input.NewPassword);
            return _database.Update(model);
        }
        public bool ResetPassword(ResetPasswordDto input)
        {
            List<IPredicate> predicates = new List<IPredicate>
            {
                 Predicates.Field<Member>(f => f.UserName, Operator.Eq, AdminUserName),
                 Predicates.Field<Member>(f => f.TenantId, Operator.Eq, _accessor.TenantId)
            };
            var admin = _database.GetList<Member>(Predicates.Group(GroupOperator.And, predicates.ToArray())).FirstOrDefault();
            if (admin == null)
                throw new UserFriendlyException("当前用户无权限");
            if (admin.Password != DESHelper.EncryptByGeneric(input.AdminPassword))
                throw new UserFriendlyException("超管密码错误");
            var model = _database.Get<Member>(input.MemberId);
            model.Password = DESHelper.EncryptByGeneric(input.NewPassword);
            return _database.Update(model);
        }
    }
}
