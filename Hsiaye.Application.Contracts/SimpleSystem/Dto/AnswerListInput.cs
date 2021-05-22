using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    public class AnswerListInput : PageInput
    {
        public int QuestionId { get; set; }
        public string SortField { get; set; }
    }
}
