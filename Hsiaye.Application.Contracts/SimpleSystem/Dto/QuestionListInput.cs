using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    public class QuestionListInput : PageInput
    {
        public int CategoryId { get; set; }
        public string Keywords { get; set; }
        public string SortField { get; set; }
    }
}
