using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    /// <summary>
    /// 登录数据
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 验证码Key
        /// </summary>
        public string VerifyKey { get; set; }
        /// <summary>
        /// 验证码Code
        /// </summary>
        public string VerifyCode { get; set; }
    }
}
