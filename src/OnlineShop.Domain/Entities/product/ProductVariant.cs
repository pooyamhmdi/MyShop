using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineShop.Domain.Entities.specification;

namespace OnlineShop.Domain.Entities.product
{
    public class ProductVariant
    {
        public long Id { get; set; }
        public string SKU { get; set; }
        public decimal PusrchasePrice { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public Product product { get; set; }
        public long ProductId { get; set; }
        public ICollection<Specification> Specification { get; set; }
        = new List<Specification>();

        public ICollection<ProductImage> ProductImages { get; set; }
        = new List<ProductImage>();
    }
}