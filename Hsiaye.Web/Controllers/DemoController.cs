using DapperExtensions;
using DapperExtensions.Predicate;
using Hsiaye.Application;
using Hsiaye.Domain;
using Hsiaye.Domain.Shared;
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
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
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
        [HttpGet]
        public IEnumerable<Demo> List(int page, int limit)
        {
            ISort sort = Predicates.Sort<Demo>(x => x.Id, false);
            var list = _database.GetPage<Demo>(null, new List<ISort> { sort }, page, limit);
            return list;
        }
        [HttpGet]
        //[Authorize]
        public string Current()
        {
            return "1";
        }
    }
}
