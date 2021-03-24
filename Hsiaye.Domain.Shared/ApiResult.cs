using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain.Shared
{
    public class ApiResult
    {
        /// <summary>
        /// 业务上的请求是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 业务约定的错误码
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 业务上的错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        public object Data { get; set; }
    }
}
