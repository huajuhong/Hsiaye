﻿using Hsiaye.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hsiaye.Web.Extensions.Filters
{
    public class ActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            context.Result = new JsonResult(new ApiResult { Code = 0, Message = "success", Data = context.Result });
        }

        public void OnActionExecuting(ActionExecutingContext context) { }
    }
}
