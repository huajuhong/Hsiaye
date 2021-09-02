using Hsiaye.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
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
                var type = objectResult.Value.GetType();
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(PageResult<>))
                {
                    JObject jObject = JObject.FromObject(objectResult.Value);
                    context.Result = new JsonResult(new ApiResult { Success = true, Code = 200, Message = "", Data = jObject["List"], Count = jObject["Count"].ToObject<int>() });
                }
                else
                {
                    context.Result = new JsonResult(new ApiResult { Success = true, Code = 200, Message = "", Data = objectResult.Value });
                }
                return;
            }

            if (context.Result is StatusCodeResult statusCodeResult)
            {
                context.Result = new JsonResult(new ApiResult { Success = true, Code = statusCodeResult.StatusCode, Message = "", });
                return;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context) { }
    }
}
