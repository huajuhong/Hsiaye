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
            Code = code;
        }
        public UserFriendlyException(string message) : base(message) { }
        public UserFriendlyException(Exception exception) : base("服务器内部错误")
        {
            string message = exception.Message;
        }
        public int Code { get; set; }
    }
}
