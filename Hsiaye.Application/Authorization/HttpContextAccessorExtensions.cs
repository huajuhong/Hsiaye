using Hsiaye.Application.Contracts;
using Hsiaye.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application
{
    public static class HttpContextAccessorExtensions
    {
        public static string GetProviderKey(this IHttpContextAccessor httpContextAccessor)
        {
            if (!httpContextAccessor.HttpContext.Request.Headers.ContainsKey("access_token"))
                return "";
            string token = httpContextAccessor.HttpContext.Request.Headers["access_token"];
            return token;
        }
    }
}
