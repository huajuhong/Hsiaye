using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    public class QuestionInput
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
    public class QuestionEditInput : QuestionInput
    {
        public int Id { get; set; }
    }
}
