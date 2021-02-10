using Hsiaye.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hsiaye.Web.Extensions.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            ApiResult result;
            if (context.Exception is UserFriendlyException userFriendly)
            {
                result = new ApiResult { Code = userFriendly.Code, Message = userFriendly.Message };
            }
            else
            {
                result = new ApiResult { Code = 500, Message = "服务器内部错误" };
            }
            context.Result = new JsonResult(result);
        }
    }
}
