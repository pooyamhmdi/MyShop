using OnlineShop.Domain.Entities.brand;

namespace OnlineShop.Domain.Entities.product
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }= string.Empty;
        public Brand Brand{ get; set; } = null!;
        public long BrandId { get; set; }
        public ICollection<ProductCategory> Categories{get; set;}
        = new List<ProductCategory>();
        public ICollection<ProductImage> Images{get; set;}
        = new List<ProductImage>();
        public ICollection<ProductVariant> Variants { get; set; }
        = new List<ProductVariant>();
    }
}