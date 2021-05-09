using Hsiaye.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    public class ProductInput
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Cover { get; set; }//封面：半角逗号分割
        public string Description { get; set; }
        public int InventoryQuantity { get; set; }//存货数量
        public ProductState State { get; set; }
        public int PromotionDiscountsId { get; set; }//活动Id
    }
    public class ProductEditInput : ProductInput
    {
        public int Id { get; set; }
    }
}
