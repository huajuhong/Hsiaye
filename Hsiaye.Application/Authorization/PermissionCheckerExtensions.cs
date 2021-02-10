using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Hsiaye.Application.Contracts.Authorization;
using Hsiaye.Domain.Shared;

namespace Hsiaye.Application.Authorization
{
    public static class PermissionCheckerExtensions
    {
        public static bool IsGranted(this IPermissionChecker permissionChecker, long memberId, bool requiresAll, params string[] permissionNames)
        {
            if (permissionNames.IsNullOrEmpty())
            {
                return true;
            }

            if (requiresAll)
            {
                foreach (var permissionName in permissionNames)
                {
                    if (!(permissionChecker.IsGranted(memberId, permissionName)))
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                foreach (var permissionName in permissionNames)
                {
                    if (permissionChecker.IsGranted(memberId, permissionName))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
        public static bool IsGranted(this IPermissionChecker permissionChecker, bool requiresAll, params string[] permissionNames)
        {
            if (permissionNames.IsNullOrEmpty())
            {
                return true;
            }

            if (requiresAll)
            {
                foreach (var permissionName in permissionNames)
                {
                    if (!(permissionChecker.IsGranted(permissionName)))
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                foreach (var permissionName in permissionNames)
                {
                    if (permissionChecker.IsGranted(permissionName))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
        public static void Authorize(this IPermissionChecker permissionChecker, params string[] permissionNames)
        {
            Authorize(permissionChecker, false, permissionNames);
        }
        public static void Authorize(this IPermissionChecker permissionChecker, bool requireAll, params string[] permissionNames)
        {
            if (IsGranted(permissionChecker, requireAll, permissionNames))
            {
                return;
            }

            if (requireAll)
            {
                string message = $"必须授予所有这些权限：{string.Join(',', permissionNames)}";
                throw new UserFriendlyException(411, message);
            }
            else
            {
                string message = $"必须至少授予其中一个权限：{string.Join(',', permissionNames)}";
                throw new UserFriendlyException(412, message);
            }
        }
    }
}
