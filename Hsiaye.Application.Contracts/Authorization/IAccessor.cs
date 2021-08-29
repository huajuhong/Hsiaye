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
        int MemberId { get; }
        Member Member { get; }
        int OrganizationUnitId { get; }
        Permission[] Permissions { get; }
        long[] RoleIds { get; }
        Role[] Roles { get; }
        void RoleAuthorize(params string[] roleNames);
    }
}
