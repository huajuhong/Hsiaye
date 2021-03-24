using Hsiaye.Application.Authorization;
using Hsiaye.Dapper;
using Hsiaye.Domain.Shared;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hsiaye.Web.Extensions.Mvc;
using Microsoft.AspNetCore.Mvc;
using Hsiaye.Domain;
using Hsiaye.Domain.Members;
using Microsoft.Extensions.Caching.Memory;
using Hsiaye.Application.Contracts.Members.Dto;
using Microsoft.Extensions.Primitives;

namespace Hsiaye.Web.Extensions.Filters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly IMemoryCache _cache;
        private readonly IDatabase _database;

        public AuthorizationFilter(IDatabase database, IMemoryCache cache)
        {
            _database = database;
            _cache = cache;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isDefined = context.ActionDescriptor.GetMethodInfo().IsDefined(typeof(AuthorizeAttribute), false);
            if (!isDefined)
                return;
            if (!context.HttpContext.Request.Headers.TryGetValue("token", out StringValues token) || StringValues.IsNullOrEmpty(token))
            {
                context.Result = new JsonResult(new ApiResult { Success = false, ErrorCode = 402, ErrorMessage = "token缺失" });
                return;
            }

            var memberDto = _cache.Get<MemberDto>(token);
            if (memberDto == null)
            {
                context.Result = new JsonResult(new ApiResult { Success = false, ErrorCode = 402, ErrorMessage = "token无效" });
                return;
            }
        }
    }
}
