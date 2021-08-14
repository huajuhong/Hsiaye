using Hsiaye.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hsiaye.Web.Extensions
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            ApiResult result;
            if (context.Exception is UserFriendlyException userFriendly)
            {
                result = new ApiResult { Success = true, Code = userFriendly.Code, Message = userFriendly.Message };
            }
            else
            {
#if DEBUG
                result = new ApiResult { Success = true, Code = 500, Message = context.Exception.Message };
#else
                result = new ApiResult { Success = false, ErrorCode = 500, ErrorMessage = "服务器内部错误" };
#endif
            }
            context.Result = new JsonResult(result);
            context.ExceptionHandled = true;
        }
    }
}
