using OnlineShop.Domain.Common;
using OnlineShop.Domain.Entities.product;

namespace OnlineShop.Domain.Entities.category
{
    public class Category:BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Category? ParentCategory{get; set;}
        public long? ParentCategoryId{get; set;}
        public ICollection<Category> SubCategories{get; set;}
        = new List<Category>();
        public ICollection<ProductCategory> Products{get; set;}
        = new List<ProductCategory>();

    }
}