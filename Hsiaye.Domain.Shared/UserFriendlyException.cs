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
        public int Code { get; set; }
    }
}
