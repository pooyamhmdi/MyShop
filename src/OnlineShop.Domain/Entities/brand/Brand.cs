using OnlineShop.Domain.Common;
using OnlineShop.Domain.Entities.product;

namespace OnlineShop.Domain.Entities.brand
{
    public class Brand:BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; }
        = new List<Product>();
    }
}