using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hsiaye.Domain;
using Hsiaye.Domain.Authorization;
using Hsiaye.Domain.Members;
using Hsiaye.Domain.Roles;

namespace Hsiaye.Application.Contracts.Authorization
{
    public interface IAccessor
    {
        int TenantId { get; }
        long MemberId { get; }
        Member Member { get; }
        Permission[] Permissions { get; }
        int[] RoleIds { get; }
        Role[] Roles { get; }
        void RoleAuthorize(params string[] roleNames);
    }
}
