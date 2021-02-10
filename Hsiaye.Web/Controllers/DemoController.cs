﻿using Hsiaye.Application.Authorization;
using Hsiaye.Dapper;
using Hsiaye.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hsiaye.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class DemoController : ControllerBase
    {
        private readonly IDatabase _database;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DemoController(IDatabase database, IHttpContextAccessor httpContextAccessor)
        {
            _database = database;
            _httpContextAccessor = httpContextAccessor;
        }


        //http://localhost:55448/demo/list?page=0&limit=3
        public IEnumerable<Demo> List(int page, int limit)
        {
            ISort sort = Predicates.Sort<Demo>(x => x.Id, false);
            var list = _database.GetPage<Demo>(null, new List<ISort> { sort }, page, limit);
            return list;
        }
        [Authorize]
        public string Current()
        {
            return "1";
        }
    }
}