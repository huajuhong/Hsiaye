using Hsiaye.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hsiaye.Web.Extensions
{
    public class ActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

            if (context.Result is ObjectResult objectResult)
            {
                context.Result = new JsonResult(new ApiResult { Success = true, Code = 200, Message = "success", Data = objectResult.Value });
                return;
            }

            if (context.Result is StatusCodeResult statusCodeResult)
            {
                context.Result = new JsonResult(new ApiResult { Success = true, Code = statusCodeResult.StatusCode, Message = "success", });
                return;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context) { }
    }
}
