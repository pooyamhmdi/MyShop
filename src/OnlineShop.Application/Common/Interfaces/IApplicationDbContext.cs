using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities.product;
using OnlineShop.Domain.Entities.brand;
using OnlineShop.Domain.Entities.category;
using OnlineShop.Domain.Entities.specification;
namespace OnlineShop.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Product> Products { get; }
        DbSet<ProductImage> ProductImages { get; }
        DbSet<ProductVariant> ProductVariants { get; }
        DbSet<ProductCategory> ProductCategories { get; }

        DbSet<Category> Categories { get; }
        DbSet<Brand> Brands { get; }

        DbSet<Specification> Specifications { get; }
        DbSet<SpecificationOption> SpecificationOptions { get; }
        DbSet<VariantSpecificationValue> VariantSpecificationValues { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}