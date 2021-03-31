using Hsiaye.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Domain
{
    /// <summary>
    /// 登录尝试
    /// </summary>
    public class MemberLoginAttempt
    {
        public long Id { get; set; }
        public string BrowserInfo { get; set; }
        public string ClientIpAddress { get; set; }
        public string ClientName { get; set; }
        public bool Success { get; set; }
        public long MemberId { get; set; }
        public string UserName { get; set; }
    }
}
