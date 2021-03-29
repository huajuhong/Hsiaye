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
        public static string GetToken(this IHttpContextAccessor httpContextAccessor)
        {
            if (!httpContextAccessor.HttpContext.Request.Headers.ContainsKey("token"))
                return "";
            string token = httpContextAccessor.HttpContext.Request.Headers["token"];
            return token;
        }
    }
}
