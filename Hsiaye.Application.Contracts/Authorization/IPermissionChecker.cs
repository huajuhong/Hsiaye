using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    public interface IPermissionChecker
    {
        /// <summary>
        /// Checks if current user is granted for a permission.
        /// </summary>
        /// <param name="permissionName">Name of the permission</param>
        bool IsGranted(string permissionName);


        /// <summary>
        /// Checks if a user is granted for a permission.
        /// </summary>
        /// <param name="memberId">User to check</param>
        /// <param name="permissionName">Name of the permission</param>
        bool IsGranted(int memberId, string permissionName);
    }
}
