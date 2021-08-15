﻿using Hsiaye.Application;
using Hsiaye.Application.Contracts;
using DapperExtensions;using DapperExtensions.Predicate;
using Hsiaye.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Hsiaye.Web.Extensions
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly IMemoryCache _cache;
        //private readonly IDatabase _database;

        public AuthorizationFilter(/*IDatabase database, */IMemoryCache cache)
        {
            //_database = database;
            _cache = cache;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isDefined = context.ActionDescriptor.GetMethodInfo().IsDefined(typeof(AuthorizeAttribute), false);
            if (!isDefined)
                return;
            if (!context.HttpContext.Request.Headers.TryGetValue("token", out StringValues token) || StringValues.IsNullOrEmpty(token))
            {
                context.Result = new JsonResult(new ApiResult { Success = false, Code = 402, Message = "token缺失" });
                return;
            }
            var memberDto = _cache.Get<MemberDto>(token.ToString());
            if (memberDto == null)
            {
                context.Result = new JsonResult(new ApiResult { Success = false, Code = 402, Message = "token无效" });
                return;
            }
        }
    }
}
