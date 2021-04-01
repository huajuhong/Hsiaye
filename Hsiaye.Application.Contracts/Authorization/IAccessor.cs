using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hsiaye.Domain;

namespace Hsiaye.Application.Contracts
{
    public interface IAccessor
    {
        string ProviderKey { get; }
        int TenantId { get; }
        long MemberId { get; }
        Member Member { get; }
        Permission[] Permissions { get; }
        int[] RoleIds { get; }
        Role[] Roles { get; }
        void RoleAuthorize(params string[] roleNames);
    }
}
