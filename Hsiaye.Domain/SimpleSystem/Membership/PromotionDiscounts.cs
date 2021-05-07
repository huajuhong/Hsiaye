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
        public decimal Discount { get; set; }//折扣，5.5折=5.5

    }
    public enum PromotionDiscountsState
    {
        未开始 = 0,
        进行中 = 1,
        已结束 = 2,
    }
    public enum PromotionDiscountsRule
    {
        无折扣 = 0,
        满减 = 1,
        满返 = 2,
        满折 = 3,
        直降 = 4
    }
}
