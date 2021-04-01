using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hsiaye.Domain
{
    public class MemberToken
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public long MemberId { get; set; }
        //登录提供程序
        [StringLength(64)]
        public string LoginProvider { get; set; }

        //登录提供程序的Key
        [StringLength(64)]
        public string ProviderKey { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}