﻿using Hsiaye.Application.Contracts;
using DapperExtensions;
using Hsiaye.Domain;
using Hsiaye.Domain.Shared;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using DapperExtensions.Predicate;

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
        public int MemberId
        {
            get
            {
                var list = _database.GetList<MemberToken>(Predicates.Field<MemberToken>(f => f.ProviderKey, Operator.Eq, ProviderKey));
                if (list.Any())
                {
                    return list.FirstOrDefault().MemberId;
                }
                else
                {
                    //todo:ProviderKey失效
                    return 0;
                }
            }
        }
        public Member Member => _database.Get<Member>(MemberId);
        public int OrganizationUnitId
        {
            get
            {

                var list = _database.GetList<MemberOrganizationUnit>(Predicates.Field<MemberOrganizationUnit>(f => f.MemberId, Operator.Eq, MemberId));
                if (list.Any())
                {
                    return list.FirstOrDefault().OrganizationUnitId;
                }
                else
                {
                    //todo:ProviderKey失效
                    return 0;
                }
            }
        }
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
        public long[] RoleIds => _database.GetList<MemberRole>(Predicates.Field<MemberRole>(f => f.MemberId, Operator.Eq, MemberId)).Select(x => x.RoleId).ToArray();
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