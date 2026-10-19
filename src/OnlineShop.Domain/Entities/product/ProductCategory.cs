using OnlineShop.Domain.Entities.category;

namespace OnlineShop.Domain.Entities.product
{
    public class ProductCategory
    {
        public Product Product { get; set; } = null!;
        public long ProductId { get; set; }
        public Category Category { get; set; } = null!;
        public long CategoryId { get; set; }
    }
}