using Hsiaye.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    //提现
    public class MembershipWithdrawalInput
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public PayType PayType { get; set; }
    }
}
