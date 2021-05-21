using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain.Shared
{
    public interface IPagedResult<T> : IList<T>
    {
        public int Total { get; set; }
    }
}
