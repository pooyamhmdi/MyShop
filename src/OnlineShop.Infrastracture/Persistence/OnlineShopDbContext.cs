using OnlineShop.Domain.Entities.product;
using OnlineShop.Domain.Entities.category;
using OnlineShop.Domain.Entities.brand;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities.specification;
using OnlineShop.Domain.Common;
using OnlineShop.Application.Common.Interfaces;

namespace OnlineShop.Infrastracture.Persistence
{
    public class OnlineShopDbContext : DbContext, IApplicationDbContext
    {
        public OnlineShopDbContext(DbContextOptions<OnlineShopDbContext> options)
        : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Specification> Specifications { get; set; }
        public DbSet<SpecificationOption> SpecificationOptions { get; set; }
        public DbSet<VariantSpecificationValue> VariantSpecificationValues { get; set; }
        public DbSet<Brand> Brands { get; set; }

        public override int SaveChanges()
        {
            UpdateAuditFields();

            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();

            return base.SaveChangesAsync(cancellationToken);
        }
        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries<BaseEntity>()
                .Where(entry =>
                    entry.State == EntityState.Added ||
                    entry.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity.CreatedOn == default)
                    {
                        entry.Entity.CreatedOn = DateTime.UtcNow;
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(entity => entity.CreatedOn).IsModified = false;
                    entry.Entity.LastModifiedOn = DateTime.UtcNow;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductCategory>()
            .HasKey(pc => new { pc.ProductId, pc.CategoryId });

            modelBuilder.Entity<ProductVariant>()
            .Property(pv => pv.Price)
            .HasPrecision(18, 2);
            modelBuilder.Entity<ProductVariant>()
            .Property(pv => pv.PurchasePrice)
            .HasPrecision(18, 2);

            modelBuilder.Entity<Category>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

            ApplyQueryFilter(modelBuilder);
        }

        private void ApplyQueryFilter(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Product>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<ProductVariant>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Brand>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Specification>()
                .HasQueryFilter(p => !p.IsDeleted);
        }
    }
}