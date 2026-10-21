using OnlineShop.Domain.Common;

namespace OnlineShop.Domain.Entities.product
{
    public class ProductImage : BaseEntity
    {
        public string ImageUrl { get; set; } = string.Empty;

        public bool IsMain { get; set; }

        public int DisplayOrder { get; set; }

        public Product? Product { get; set; }

        public long? ProductId { get; set; }

        public ProductVariant? ProductVariant { get; set; }

        public long? ProductVariantId { get; set; }
    }
}
