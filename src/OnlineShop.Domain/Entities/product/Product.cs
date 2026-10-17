using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineShop.Domain.Entities.brand;
using OnlineShop.Domain.Entities.category;

namespace OnlineShop.Domain.Entities.product
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; }
        public Category Category{get ; set;}
        public long CategoryId { get; set; }
        public Brand Brand{ get; set; }
        public long BrandId { get; set; }
        public ProductImage productImage { get; set; }
        public long productImageId { get; set; }
        public ICollection<ProductVariant> Variants { get; set; }
        = new List<ProductVariant>();
    }
}