using Hsiaye.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    public class PromotionDiscountsInput
    {
        public string Name { get; set; }
        public PromotionDiscountsState State { get; set; }
        public bool Approved { get; set; }//是否已审
        public PromotionDiscountsRule Rule { get; set; }
        public decimal RuleAmount { get; set; }
        public decimal Discount { get; set; }//折扣，5.5折=5.5
    }
    public class PromotionDiscountsEditInput: PromotionDiscountsInput
    {
        public int Id { get; set; }
    }
}
