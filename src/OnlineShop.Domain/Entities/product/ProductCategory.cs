using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineShop.Domain.Entities.category;

namespace OnlineShop.Domain.Entities.product
{
    public class ProductCategory
    {
        public Product Product { get; set; }
        public long ProductId { get; set; }
        public Category Category { get; set; }
        public long CategoryId { get; set; }
    }
}