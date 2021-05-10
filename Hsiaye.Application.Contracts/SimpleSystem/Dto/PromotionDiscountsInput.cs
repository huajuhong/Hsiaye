﻿using Hsiaye.Domain;
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
        public PromotionDiscountsRule Rule { get; set; }
        public decimal RuleAmount { get; set; }
        public decimal RuleDiscount { get; set; }//促销活动规则的折扣。例如：5.5折，则值为5.5，取值范围0-10
        public decimal RuleDiscountAmount { get; set; }//促销活动规则的优惠金额。例如：满减（满100减20）、直降（直降20），则值为20
        public bool Approved { get; set; }//是否已审
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
    public class PromotionDiscountsEditInput: PromotionDiscountsInput
    {
        public int Id { get; set; }
    }
}
