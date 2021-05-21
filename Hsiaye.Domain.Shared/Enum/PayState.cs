using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain.Shared
{
    public enum PayState
    {
        待支付 = 1,
        支付中 = 2,
        支付成功 = 3,
        支付失败 = 4,
    }
}
