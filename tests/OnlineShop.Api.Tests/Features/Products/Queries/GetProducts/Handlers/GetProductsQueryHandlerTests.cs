using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities.brand;
using OnlineShop.Domain.Entities.product;
using OnlineShop.Infrastracture.Persistence;
using OnlineShop.Application.Features.Products.Queries.GetProducts;
using OnlineShop.Domain.Entities.category;

namespace OnlineShop.Api.Tests.Features.Products.Queries.GetProducts.Handlers;

public class GetProductsQueryHandlerTests : IDisposable
{
    private readonly OnlineShopDbContext _context;

    public GetProductsQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<OnlineShopDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new OnlineShopDbContext(options);
    }

    [Fact]
    public async Task Handle_ShouldReturnProductsWithVariantsAndImages()
    {
        // Arrange
        var brand = new Brand
        {
            Name = "ASUS"
        };

        _context.Brands.Add(brand);
        await _context.SaveChangesAsync();

        var product = new Product
        {
            Name = "ASUS TUF Gaming Laptop",
            Description = "Gaming Laptop",
            BrandId = brand.Id,
            Brand = brand
        };

        var variant = new ProductVariant
        {
            SKU = "ASUS-TUF-001",
            Price = 55000000,
            PurchasePrice = 50000000,
            Stock = 10,
            Product = product
        };

        product.Variants.Add(variant);

        // Product Images
        product.Images.Add(new ProductImage
        {
            ImageUrl = "product-secondary.jpg",
            IsMain = false,
            DisplayOrder = 1
        });

        product.Images.Add(new ProductImage
        {
            ImageUrl = "product-main.jpg",
            IsMain = true,
            DisplayOrder = 2
        });

        // Variant Images
        variant.Images.Add(new ProductImage
        {
            ImageUrl = "variant-secondary.jpg",
            IsMain = false,
            DisplayOrder = 1
        });

        variant.Images.Add(new ProductImage
        {
            ImageUrl = "variant-main.jpg",
            IsMain = true,
            DisplayOrder = 2
        });

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var handler = new GetProductsQueryHandler(_context);
        var request = new GetProductsQuery();

        // Act
        var result = await handler.Handle(
            request,
            CancellationToken.None);

        // Assert
        Assert.Equal(1, result.TotalCount);
        var returnedProduct = Assert.Single(result.Items);

        Assert.Equal("ASUS TUF Gaming Laptop", returnedProduct.Name);
        Assert.Equal(brand.Id, returnedProduct.BrandId);

        // Check Product Images
        Assert.Equal(2, returnedProduct.Images.Count);

        Assert.Equal(
            "product-main.jpg",
            returnedProduct.Images[0].ImageUrl);

        Assert.Equal(
            "product-secondary.jpg",
            returnedProduct.Images[1].ImageUrl);

        // Check Variant
        var returnedVariant = Assert.Single(returnedProduct.Variants);

        Assert.Equal("ASUS-TUF-001", returnedVariant.SKU);
        Assert.Equal(55000000, returnedVariant.Price);
        Assert.Equal(10, returnedVariant.Stock);

        // Check Variant Images
        Assert.Equal(2, returnedVariant.Images.Count);

        Assert.Equal(
            "variant-main.jpg",
            returnedVariant.Images[0].ImageUrl);

        Assert.Equal(
            "variant-secondary.jpg",
            returnedVariant.Images[1].ImageUrl);
    }

    [Fact]
    public async Task Handle_SearchTermByProductName_ReturnsMatchingProducts()
    {
        // Arrange
        var brand = new Brand
        {
            Name = "ASUS"
        };

        _context.Brands.Add(brand);
        await _context.SaveChangesAsync();

        _context.Products.AddRange(
            new Product
            {
                Name = "ROG Gaming Laptop",
                BrandId = brand.Id
            },
            new Product
            {
                Name = "TUF Gaming Laptop",
                BrandId = brand.Id
            });

        await _context.SaveChangesAsync();

        var handler = new GetProductsQueryHandler(_context);

        var request = new GetProductsQuery(
            Filter: new ProductFilter(
                SearchTerm: "ROG"
            )
        );

        // Act
        var result = await handler.Handle(
            request,
            CancellationToken.None);

        // Assert
        var product = Assert.Single(result.Items);

        Assert.Equal("ROG Gaming Laptop", product.Name);
    }

    [Fact]
    public async Task Handle_SearchTermByBrandName_ReturnsMatchingProducts()
    {
        // Arrange
        var asus = new Brand
        {
            Name = "ASUS"
        };

        var dell = new Brand
        {
            Name = "Dell"
        };

        _context.Brands.AddRange(asus, dell);
        await _context.SaveChangesAsync();

        _context.Products.AddRange(
            new Product
            {
                Name = "ROG Gaming Laptop",
                BrandId = asus.Id
            },
            new Product
            {
                Name = "Inspiron Laptop",
                BrandId = dell.Id
            });

        await _context.SaveChangesAsync();

        var handler = new GetProductsQueryHandler(_context);

        var request = new GetProductsQuery(
            Filter: new ProductFilter(
                SearchTerm: "ASUS"
            )
        );

        // Act
        var result = await handler.Handle(
            request,
            CancellationToken.None);

        // Assert
        var product = Assert.Single(result.Items);

        Assert.Equal("ROG Gaming Laptop", product.Name);
    }

    [Fact]
    public async Task Handle_BrandIdFilter_ReturnsOnlyProductsOfSelectedBrand()
    {
        // Arrange
        var asus = new Brand { Name = "ASUS" };
        var dell = new Brand { Name = "Dell" };

        _context.Brands.AddRange(asus, dell);
        await _context.SaveChangesAsync();

        _context.Products.AddRange(
            new Product
            {
                Name = "ROG Laptop",
                BrandId = asus.Id
            },
            new Product
            {
                Name = "TUF Laptop",
                BrandId = asus.Id
            },
            new Product
            {
                Name = "Inspiron Laptop",
                BrandId = dell.Id
            });

        await _context.SaveChangesAsync();

        var handler = new GetProductsQueryHandler(_context);

        var request = new GetProductsQuery(
            Filter: new ProductFilter(
                BrandId: asus.Id
            )
        );

        // Act
        var result = await handler.Handle(
            request,
            CancellationToken.None);

        // Assert
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(2, result.Items.Count);

        Assert.All(result.Items, product =>
            Assert.Equal(asus.Id, product.BrandId));
    }

    [Fact]
    public async Task Handle_CategoryIdFilter_ReturnsProductsFromSelectedCategoryAndDescendants()
    {
        // Arrange
        var laptop = new Category
        {
            Name = "Laptop"
        };

        var asus = new Category
        {
            Name = "ASUS",
            ParentCategory = laptop
        };

        var rog = new Category
        {
            Name = "ROG",
            ParentCategory = asus
        };

        var mobile = new Category
        {
            Name = "Mobile"
        };

        _context.Categories.AddRange(laptop, asus, rog, mobile);
        await _context.SaveChangesAsync();

        var products = new List<Product>
    {
        new() { Name = "Laptop Product" },
        new() { Name = "ASUS Product" },
        new() { Name = "ROG Product" },
        new() { Name = "Mobile Product" }
    };

        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        _context.ProductCategories.AddRange(
            new ProductCategory
            {
                ProductId = products[0].Id,
                CategoryId = laptop.Id
            },
            new ProductCategory
            {
                ProductId = products[1].Id,
                CategoryId = asus.Id
            },
            new ProductCategory
            {
                ProductId = products[2].Id,
                CategoryId = rog.Id
            },
            new ProductCategory
            {
                ProductId = products[3].Id,
                CategoryId = mobile.Id
            });

        await _context.SaveChangesAsync();

        var handler = new GetProductsQueryHandler(_context);

        var request = new GetProductsQuery(
            Filter: new ProductFilter(
                CategoryId: laptop.Id
            )
        );

        // Act
        var result = await handler.Handle(
            request,
            CancellationToken.None);

        // Assert
        Assert.Equal(3, result.TotalCount);

        Assert.Contains(result.Items, p => p.Name == "Laptop Product");
        Assert.Contains(result.Items, p => p.Name == "ASUS Product");
        Assert.Contains(result.Items, p => p.Name == "ROG Product");

        Assert.DoesNotContain(result.Items, p => p.Name == "Mobile Product");
    }

    [Fact]
    public async Task Handle_CategoryIdFilter_ShouldReturnProductsUpToFourDescendantLevels()
    {
        // Arrange
        var level1 = new Category { Name = "Laptop" };
        var level2 = new Category { Name = "ASUS", ParentCategory = level1 };
        var level3 = new Category { Name = "ROG", ParentCategory = level2 };
        var level4 = new Category { Name = "Gaming", ParentCategory = level3 };
        var level5 = new Category { Name = "TUF", ParentCategory = level4 };
        var level6 = new Category { Name = "Accessories", ParentCategory = level5 };

        _context.Categories.AddRange(
            level1, level2, level3, level4, level5, level6);

        await _context.SaveChangesAsync();

        var categories = new[]
        {
        level1, level2, level3, level4, level5, level6
    };

        var products = categories.Select((category, index) => new Product
        {
            Name = $"Product Level {index + 1}",
            Categories =
            [
                new ProductCategory
            {
                CategoryId = category.Id
            }
            ]
        }).ToList();

        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        var handler = new GetProductsQueryHandler(_context);

        var request = new GetProductsQuery(
            Filter: new ProductFilter(CategoryId: level1.Id)
        );

        // Act
        var result = await handler.Handle(
            request,
            CancellationToken.None);

        // Assert
        Assert.Equal(5, result.TotalCount);

        Assert.Contains(result.Items, p => p.Name == "Product Level 1");
        Assert.Contains(result.Items, p => p.Name == "Product Level 2");
        Assert.Contains(result.Items, p => p.Name == "Product Level 3");
        Assert.Contains(result.Items, p => p.Name == "Product Level 4");
        Assert.Contains(result.Items, p => p.Name == "Product Level 5");

        Assert.DoesNotContain(
            result.Items,
            p => p.Name == "Product Level 6");
    }

    [Fact]
    public async Task Handle_SortByName_ReturnsProductsInAlphabeticalOrder()
    {
        // Arrange
        var brand = new Brand
        {
            Name = "ASUS"
        };

        _context.Brands.Add(brand);
        await _context.SaveChangesAsync();

        _context.Products.AddRange(
            new Product
            {
                Name = "ROG Laptop",
                BrandId = brand.Id
            },
            new Product
            {
                Name = "ASUS Laptop",
                BrandId = brand.Id
            },
            new Product
            {
                Name = "TUF Laptop",
                BrandId = brand.Id
            });

        await _context.SaveChangesAsync();

        var handler = new GetProductsQueryHandler(_context);

        var request = new GetProductsQuery(
            Filter: new ProductFilter(
                SortBy: ProductSortBy.Name
            )
        );

        // Act
        var result = await handler.Handle(
            request,
            CancellationToken.None);

        // Assert
        Assert.Equal(3, result.Items.Count);

        Assert.Equal("ASUS Laptop", result.Items[0].Name);
        Assert.Equal("ROG Laptop", result.Items[1].Name);
        Assert.Equal("TUF Laptop", result.Items[2].Name);
    }

    [Fact]
    public async Task Handle_SortByOldest_ReturnsOldestProductsFirst()
    {
        // Arrange
        var brand = new Brand { Name = "ASUS" };

        _context.Brands.Add(brand);
        await _context.SaveChangesAsync();

        var products = new List<Product>
    {
        new()
        {
            Name = "New Product",
            BrandId = brand.Id,
            CreatedOn = new DateTime(2026, 3, 1)
        },
        new()
        {
            Name = "Old Product",
            BrandId = brand.Id,
            CreatedOn = new DateTime(2026, 1, 1)
        },
        new()
        {
            Name = "Middle Product",
            BrandId = brand.Id,
            CreatedOn = new DateTime(2026, 2, 1)
        }
    };

        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        var handler = new GetProductsQueryHandler(_context);

        var request = new GetProductsQuery(
            Filter: new ProductFilter(
                SortBy: ProductSortBy.Oldest
            )
        );

        // Act
        var result = await handler.Handle(
            request,
            CancellationToken.None);

        // Assert
        Assert.Equal(3, result.Items.Count);
        Assert.Equal("Old Product", result.Items[0].Name);
        Assert.Equal("Middle Product", result.Items[1].Name);
        Assert.Equal("New Product", result.Items[2].Name);
    }

    [Fact]
    public async Task Handle_SortByNewest_ReturnsNewestProductsFirst()
    {
        // Arrange
        var brand = new Brand { Name = "ASUS" };

        _context.Brands.Add(brand);
        await _context.SaveChangesAsync();

        var products = new List<Product>
    {
        new()
        {
            Name = "Old Product",
            BrandId = brand.Id,
            CreatedOn = new DateTime(2026, 1, 1)
        },
        new()
        {
            Name = "New Product",
            BrandId = brand.Id,
            CreatedOn = new DateTime(2026, 3, 1)
        },
        new()
        {
            Name = "Middle Product",
            BrandId = brand.Id,
            CreatedOn = new DateTime(2026, 2, 1)
        }
    };

        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        var handler = new GetProductsQueryHandler(_context);

        var request = new GetProductsQuery(
            Filter: new ProductFilter(
                SortBy: ProductSortBy.Newest
            )
        );

        // Act
        var result = await handler.Handle(
            request,
            CancellationToken.None);

        // Assert
        Assert.Equal(3, result.Items.Count);
        Assert.Equal("New Product", result.Items[0].Name);
        Assert.Equal("Middle Product", result.Items[1].Name);
        Assert.Equal("Old Product", result.Items[2].Name);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

}
