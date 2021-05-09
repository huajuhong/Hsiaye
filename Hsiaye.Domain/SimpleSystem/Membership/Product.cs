using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    //机构商品
    public class Product
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreateMemberId { get; set; }
        public int OrganizationUnitId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Cover { get; set; }//封面：半角逗号分割
        public string Description { get; set; }
        public int InventoryQuantity { get; set; }//存货数量
        public ProductState State { get; set; }
        public int PromotionDiscountsId { get; set; }//活动Id
    }
    //商品规则
    public class ProductSpecification
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int SortOrder { get; set; } //显示顺序
    }
    public enum ProductState
    {
        未知 = 0,
        上架 = 1,
        下架 = 2
    }
}
