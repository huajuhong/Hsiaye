using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Domain.Shared
{
    public enum MultiTenancySides
    {
        // 租户方
        Tenant = 1,
        // 业主方（租赁所有人）
        Host = 2
    }
}
