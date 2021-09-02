using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain.Shared
{
    public class UserFriendlyException : Exception
    {
        public UserFriendlyException(int code, string message) : base(message)
        {
            this.Code = code;
        }
        public UserFriendlyException(string message) : base(message) { this.Code = 500; }
        public UserFriendlyException(Exception exception) : base("服务器内部错误")
        {
            this.Code = 500;
            string message = exception.Message;
            //todo 日志记录错误信息
        }
        public int Code { get; set; }
    }
}
