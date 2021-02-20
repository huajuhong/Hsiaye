using Hsiaye.Application.Contracts.Authorization;
using Hsiaye.Dapper;
using Hsiaye.Domain.Authorization;
using Hsiaye.Domain.Members;
using Hsiaye.Domain.Roles;
using Hsiaye.Domain.Shared;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Hsiaye.Application.Authorization
{
    public class Accessor : IAccessor
    {
        private readonly IDatabase _database;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Accessor(IDatabase database, IHttpContextAccessor httpContextAccessor)
        {
            _database = database;
            _httpContextAccessor = httpContextAccessor;
        }

        public long MemberId => _database.GetList<MemberToken>(Predicates.Field<MemberToken>(f => f.ProviderKey, Operator.Eq, _httpContextAccessor.GetToken())).FirstOrDefault().MemberId;
        public Member Member => _database.Get<Member>(MemberId);
        public int TenantId => Member.TenantId;
        public Permission[] Permissions => _database.GetList<Permission>(Predicates.Field<Permission>(f => f.MemberId, Operator.Eq, MemberId)).ToArray();
        public int[] RoleIds => _database.GetList<Member_Role>(Predicates.Field<Member_Role>(f => f.MemberId, Operator.Eq, MemberId)).Select(x => x.RoleId).ToArray();
        public Role[] Roles => _database.GetList<Role>(Predicates.Field<Role>(f => f.Id, Operator.Eq, RoleIds)).ToArray();
        public void RoleAuthorize(params string[] roleNames)
        {
            if (Roles != null && !Roles.Any(x => roleNames.Contains(x.Name)))
            {
                throw new UserFriendlyException($"可操作角色：{string.Join(",", Roles.Select(r => r.Description))}");
            }
        }
    }
}