using OnlineShop.Domain.Common;
using OnlineShop.Domain.Entities.specification;

namespace OnlineShop.Domain.Entities.product
{
    public class ProductVariant: BaseEntity
    {
        public string SKU { get; set; } = string.Empty;
        public decimal PurchasePrice { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public Product Product { get; set; } = null!;
        public long ProductId { get; set; }
        public ICollection<VariantSpecificationValue> SpecificationValues { get; set; }
        = new List<VariantSpecificationValue>();
        public ICollection<ProductImage> Images { get; set; }
        = new List<ProductImage>();
    }
}