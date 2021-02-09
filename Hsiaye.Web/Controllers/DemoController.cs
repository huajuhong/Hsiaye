using Hsiaye.Dapper;
using Hsiaye.Domain;
using Microsoft.AspNetCore.Mvc;
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

        public DemoController(IDatabase database)
        {
            _database = database;
        }

        //http://localhost:55448/demo/list?page=0&limit=3
        public IEnumerable<Demo> List(int page, int limit)
        {
            ISort sort = Predicates.Sort<Demo>(x => x.Id, false);
            var list = _database.GetPage<Demo>(null, new List<ISort> { sort }, page, limit);
            return list;
        }
    }
}
