using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hsiaye.Domain
{
    public class Tenant
    {

        public const int MaxTenancyNameLength = 64;

        public const int MaxConnectionStringLength = 1024;

        public const string DefaultTenantName = "Default";

        public const string TenancyNameRegex = "^[a-zA-Z][a-zA-Z0-9_-]{1,}$";

        public const int MaxNameLength = 128;

        // 租赁名称。此属性是此租户的唯一名称。
        //[Required]
        [StringLength(64)]
        public string TenancyName { get; set; }
        // Display name of the Tenant.
        //[Required]
        //[StringLength(128)]
        public string Name { get; set; }
        // 租户数据库的加密连接字符串。
        //[StringLength(1024)]
        public string ConnectionString { get; set; }
        // 此租户是否处于活动状态？如果不是那么此租户的任何用户都不能使用应用程序。
        public bool IsActive { get; set; }
    }
}
