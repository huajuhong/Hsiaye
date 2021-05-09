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
                Phone = string.Empty,
            };
            try
            {
                _database.BeginTransaction();
                long id = _database.Insert(model);
                List<IPredicate> predicates = new List<IPredicate>
                {
                    Predicates.Field<Role>(f => f.Name, Operator.Eq, input.RoleNames),
                };

                var createRoles = _database.GetList<Role>(Predicates.Group(GroupOperator.And, predicates.ToArray()));

                if (createRoles != null)
                {
                    List<MemberRole> memberRoles = new List<MemberRole>();
                    foreach (var createRole in createRoles)
                    {
                        memberRoles.Add(new MemberRole
                        {
                            CreatorMemberId = _accessor.MemberId,
                            MemberId = id,
                            RoleId = createRole.Id,
                        });
                    }
                    _database.Insert(memberRoles);
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
                List<MemberRole> memberRoles = _database.GetList<MemberRole>(Predicates.Field<MemberRole>(f => f.MemberId, Operator.Eq, input.Id)).ToList();
                _database.Delete(memberRoles);
                memberRoles = new List<MemberRole>();

                var predicate = new List<IPredicate>();
                predicate.Add(Predicates.Field<Role>(f => f.Name, Operator.Eq, input.RoleNames));
                var roles = _database.GetList<Role>(predicate);
                foreach (var role in roles)
                {
                    memberRoles.Add(new MemberRole
                    {
                        CreatorMemberId = _accessor.MemberId,
                        MemberId = input.Id,
                        RoleId = role.Id,
                    });
                }
                _database.Insert(memberRoles);
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
            var roleIds = _database.GetList<MemberRole>(Predicates.Field<MemberRole>(f => f.MemberId, Operator.Eq, id)).Select(r => r.RoleId);
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
                 Predicates.Field<Member>(f => f.UserName, Operator.Eq,PermissionNames.AdminUserName),
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
