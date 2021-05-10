using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    //促销活动
    public class PromotionDiscounts
    {
        //名称/状态/活动时间开始-结束/审核状态
        //活动规则：
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int OrganizationUnitId { get; set; }
        public string Name { get; set; }
        public PromotionDiscountsRule Rule { get; set; }
        public decimal RuleAmount { get; set; }
        public decimal RuleDiscount { get; set; }//促销活动规则的折扣。例如：5.5折，则值为5.5，取值范围0-10
        public decimal RuleDiscountAmount { get; set; }//促销活动规则的优惠金额。例如：满减（满100减20）、直降（直降20），则值为20
        public bool Approved { get; set; }//是否已审
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
    public enum PromotionDiscountsRule
    {
        未知 = 0,
        满减 = 1,
        满折 = 2,
        直降 = 3,
        无折扣 = 4,
    }
}
