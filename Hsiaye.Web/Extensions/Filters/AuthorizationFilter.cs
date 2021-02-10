using Hsiaye.Application.Authorization;
using Hsiaye.Dapper;
using Hsiaye.Domain.Member;
using Hsiaye.Domain.Shared;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hsiaye.Web.Extensions.Mvc;

namespace Hsiaye.Web.Extensions.Filters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly IDatabase _database;

        public AuthorizationFilter(IDatabase database)
        {
            _database = database;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isDefined = context.ActionDescriptor.GetMethodInfo().IsDefined(typeof(AuthorizeAttribute), false);
            if (!isDefined)
                return;
            if (!context.HttpContext.Request.Headers.ContainsKey("token"))
            {
                throw new UserFriendlyException(401, "token缺失");
            }
            string token = context.HttpContext.Request.Headers["token"];
            if (string.IsNullOrEmpty(token))
            {
                throw new UserFriendlyException(402, "token无效");
            }
            IFieldPredicate predicate = Predicates.Field<MemberLogin>(f => f.ProviderKey, Operator.Eq, token);
            var model = _database.GetList<MemberLogin>(predicate).First();
            if (model == null)
            {
                throw new UserFriendlyException(403, "token过期");
            }
        }
    }
}
