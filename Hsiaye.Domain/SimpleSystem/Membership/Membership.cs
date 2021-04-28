using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    //会员
    public class Membership
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string Name { get; set; }
        public int Sex { get; set; }
        public string Phone { get; set; }
        public string IDCard { get; set; }
        public decimal Balance { get; set; }
    }

    //资金流水
    public class Fundsflow
    {
        public int Id { get; set; }
        public int MembershipId { get; set; }
        public int ProductId { get; set; }
        public int PromotionDiscountsId { get; set; }
        public int Type { get; set; }//业务类型
        public string Title { get; set; }//标题
        public decimal Amount { get; set; }//金额
        public decimal IncomeAmount { get; set; }//收入金额
        public decimal DisburseAmount { get; set; }//支出金额
        public decimal Balance { get; set; }//该账单成交后的余额
        public int State { get; set; }//状态
        public int PayType { get; set; }
        public string Description { get; set; }//说明
        public DateTime CreateTime { get; set; }
        public string OrderNumber { get; set; }
    }

    //促销活动
    public class PromotionDiscounts
    {
        //名称/状态/费用承担/活动时间开始-结束/审核状态
        //活动规则：满减、满返、满折、直降
    }
}
