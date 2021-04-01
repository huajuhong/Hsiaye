using Hsiaye.Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hsiaye.Domain
{
    /// <summary>
    /// 登录尝试
    /// </summary>
    public class MemberLoginAttempt
    {
        public long Id { get; set; }
        [StringLength(1024)]
        public string BrowserInfo { get; set; }
        [StringLength(64)]
        public string ClientIpAddress { get; set; }
        [StringLength(64)]
        public string ClientName { get; set; }
        public bool Success { get; set; }
        public long MemberId { get; set; }
        [StringLength(64)]
        public string UserName { get; set; }
    }
}
