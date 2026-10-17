using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineShop.Domain.Entities.product;

namespace OnlineShop.Domain.Entities.brand
{
    public class Brand
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
        = new List<Product>();

    }
}