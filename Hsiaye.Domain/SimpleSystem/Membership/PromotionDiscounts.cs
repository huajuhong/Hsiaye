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
        public PromotionDiscountsState State { get; set; }
        public bool Approved { get; set; }//是否已审
        public PromotionDiscountsRule Rule { get; set; }
        public decimal RuleAmount { get; set; }
        public decimal Discount { get; set; }//折扣或已优惠的金额。折扣：5.5折=5.5，满减（满100减20）/直降（直降20）=20

    }
    public enum PromotionDiscountsState
    {
        未知 = 0,
        未开始 = 1,
        进行中 = 2,
        已结束 = 3,
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
