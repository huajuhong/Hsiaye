using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizeAttribute : Attribute
    {
        public AuthorizeAttribute(params string[] permissions)
        {
            Permissions = permissions;
        }
        public string[] Permissions { get; }
        public bool RequireAllPermissions { get; set; }
    }
}
