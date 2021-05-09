using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{

    //资金流水
    public class MembershipFundsflow
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int MembershipId { get; set; }
        public int ProductId { get; set; }
        public int PromotionDiscountsId { get; set; }
        public MembershipFundsflowType Type { get; set; }//业务类型
        public string Title { get; set; }//标题
        public decimal Amount { get; set; }//流水金额
        public decimal IncomeAmount { get; set; }//收入金额
        public decimal DisburseAmount { get; set; }//支出金额
        public decimal Balance { get; set; }//该账单成交后的余额
        public Shared.PayState PayState { get; set; }
        public Shared.PayType PayType { get; set; }
        public string Description { get; set; }//说明
        public string OrderNumber { get; set; }
    }
    public enum MembershipFundsflowType
    {
        未知 = 0,
        充值 = 1,
        提现 = 2,
        消费 = 3,
    }
}
