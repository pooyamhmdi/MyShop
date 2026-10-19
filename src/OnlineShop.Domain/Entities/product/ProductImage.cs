namespace OnlineShop.Domain.Entities.product
{
    public class ProductImage
    {
        public long Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsMain { get; set; }
        public int DisplayOrder { get; set; }
        public Product? Product{get; set;}
        public long? ProductId{get; set;}
        public ProductVariant ProductVariant { get; set; } = null;
        public long ProductVariantId { get; set; }

    }
}