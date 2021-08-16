using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain.Shared
{
    public class PageResult<T> where T : class
    {
        public PageResult(IEnumerable<T> list, int count)
        {
            this.List = list;
            this.Count = count;
        }
        public IEnumerable<T> List { get; set; }
        public int Count { get; set; }
    }
}
