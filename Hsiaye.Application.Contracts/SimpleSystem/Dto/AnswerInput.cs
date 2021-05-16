using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    public class AnswerInput
    {
        public int QuestionId { get; set; }
        public string Description { get; set; }
    }
    public class AnswerEditInput : AnswerInput
    {
        public int Id { get; set; }
    }
}
