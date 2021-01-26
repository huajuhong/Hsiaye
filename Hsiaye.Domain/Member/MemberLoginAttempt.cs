﻿using Hsiaye.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Domain.Member
{
    /// <summary>
    /// 登录尝试
    /// </summary>
    public class MemberLoginAttempt
    {
        public string BrowserInfo { get; set; }
        public string ClientIpAddress { get; set; }
        public string ClientName { get; set; }
        public MemberLoginAttemptResult Result { get; set; }
        public long MemberId { get; set; }
        public string UserNameOrEmailAddress { get; set; }
    }
}
