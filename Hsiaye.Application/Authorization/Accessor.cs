using Hsiaye.Application.Contracts;
using Hsiaye.Dapper;
using Hsiaye.Domain;
using Hsiaye.Domain.Shared;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Hsiaye.Application
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

        public string ProviderKey => _httpContextAccessor.GetProviderKey();
        public long MemberId => _database.GetList<MemberToken>(Predicates.Field<MemberToken>(f => f.ProviderKey, Operator.Eq, _httpContextAccessor.GetProviderKey())).FirstOrDefault().MemberId;
        public Member Member => _database.Get<Member>(MemberId);
        public int TenantId => Member.TenantId;
        public Permission[] Permissions
        {
            get
            {
                List<Permission> permissions = new List<Permission>();
                var memberPermissions = _database.GetList<Permission>(Predicates.Field<Permission>(f => f.MemberId, Operator.Eq, MemberId)).ToList();
                if (memberPermissions.Any())
                {
                    foreach (var item in memberPermissions)
                    {
                        if (permissions.Exists(x => x.Name == item.Name))
                            continue;

                        permissions.Add(item);
                    }
                }

                if (this.RoleIds.Any())
                {
                    var rolePermissions = _database.GetList<Permission>(Predicates.Field<Permission>(f => f.RoleId, Operator.Eq, this.RoleIds));
                    foreach (var item in rolePermissions)
                    {
                        if (permissions.Exists(x => x.Name == item.Name))
                            continue;

                        permissions.Add(item);
                    }
                }

                return permissions.ToArray();
            }
        }
        public int[] RoleIds => _database.GetList<Member_Role>(Predicates.Field<Member_Role>(f => f.MemberId, Operator.Eq, MemberId)).Select(x => x.RoleId).ToArray();
        public Role[] Roles => _database.GetList<Role>(Predicates.Field<Role>(f => f.Id, Operator.Eq, RoleIds)).ToArray();
        public void RoleAuthorize(params string[] roleNames)
        {
            if (!Roles.Any(x => roleNames.Contains(x.Name)))
            {
                throw new UserFriendlyException($"可操作角色：{string.Join(",", Roles.Select(r => r.Description))}");
            }
        }
    }
}