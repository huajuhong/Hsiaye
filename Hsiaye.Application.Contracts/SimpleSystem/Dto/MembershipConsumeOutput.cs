using Hsiaye.Domain;
using Hsiaye.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    //消费
    public class MembershipConsumeOutput
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public PayState PayState { get; set; }
        public string Description { get; set; }//说明
    }
}
