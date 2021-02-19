using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Domain.Members
{
    public class MemberToken
    {
        //登录提供程序
        public string LoginProvider { get; set; }

        //登录提供程序的Key
        public string ProviderKey { get; set; }
        public int TenantId { get; set; }
        public long MemberId { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}