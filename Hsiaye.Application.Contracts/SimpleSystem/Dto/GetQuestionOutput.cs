using Hsiaye.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    public class GetQuestionOutput : Question
    {
        public List<Answer> Answers { get; set; }
    }
}
