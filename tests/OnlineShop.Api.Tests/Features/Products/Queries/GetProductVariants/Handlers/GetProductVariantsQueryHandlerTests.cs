using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities.brand;
using OnlineShop.Domain.Entities.category;
using OnlineShop.Domain.Entities.product;
using OnlineShop.Infrastracture.Persistence;
using OnlineShop.Application.Features.Products.Queries.GetProductVariants;

namespace OnlineShop.Api.Tests.Features.Products.Queries.GetProductVariants.Handlers;

public class GetProductVariantsQueryHandlerTests : IDisposable
{
    private readonly OnlineShopDbContext _context;

    public GetProductVariantsQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<OnlineShopDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new OnlineShopDbContext(options);
    }

    [Fact]
    public async Task Handle_ShouldReturnVariantsWithImages()
    {
        // Arrange
        var brand = new Brand { Name = "Asus" };

        var product = new Product
        {
            Name = "ASUS TUF",
            Description = "Gaming Laptop",
            Brand = brand
        };

        var variant1 = new ProductVariant
        {
            SKU = "ASUS-TUF-001",
            Price = 50000000,
            Stock = 10,
            Product = product
        };

        var variant2 = new ProductVariant
        {
            SKU = "ASUS-TUF-002",
            Price = 55000000,
            Stock = 5,
            Product = product
        };

        var image1 = new ProductImage
        {
            ImageUrl = "image-1.jpg",
            IsMain = false,
            DisplayOrder = 1,
            ProductVariant = variant1
        };

        var image2 = new ProductImage
        {
            ImageUrl = "image-2.jpg",
            IsMain = true,
            DisplayOrder = 2,
            ProductVariant = variant1
        };

        var image3 = new ProductImage
        {
            ImageUrl = "image-3.jpg",
            IsMain = false,
            DisplayOrder = 2,
            ProductVariant = variant2
        };

        var image4 = new ProductImage
        {
            ImageUrl = "image-4.jpg",
            IsMain = true,
            DisplayOrder = 1,
            ProductVariant = variant2
        };

        _context.Brands.Add(brand);
        _context.Products.Add(product);
        _context.ProductVariants.AddRange(variant1, variant2);
        _context.ProductImages.AddRange(image1, image2, image3, image4);

        await _context.SaveChangesAsync();

        // Act
        var handler = new GetProductVariantsQueryHandler(_context);

        var result = await handler.Handle(
            new GetProductVariantsQuery(),
            CancellationToken.None
        );

        // Assert
        Assert.Equal(2, result.Items.Count);

        Assert.Equal("ASUS-TUF-001", result.Items[0].SKU);
        Assert.Equal(product.Id, result.Items[0].ProductId);
        Assert.Equal(product.Name, result.Items[0].ProductName);
        Assert.Equal(2, result.Items[0].Images.Count);
        Assert.True(result.Items[0].Images[0].IsMain);

        Assert.Equal("ASUS-TUF-002", result.Items[1].SKU);
        Assert.Equal(product.Id, result.Items[1].ProductId);
        Assert.Equal(product.Name, result.Items[1].ProductName);
        Assert.Equal(2, result.Items[1].Images.Count);
        Assert.True(result.Items[1].Images[0].IsMain);
    }

    [Fact]
    public async Task Handle_ShouldFilterVariantsBySKU()
    {
        // Arrange
        var brand = new Brand { Name = "Asus" };
        var product = new Product
        {
            Name = "ASUS TUF",
            Description = "Gaming Laptop",
            Brand = brand
        };
        var variant1 = new ProductVariant
        {
            SKU = "ASUS-TUF-001",
            Price = 50000000,
            Stock = 10,
            Product = product
        };
        var variant2 = new ProductVariant
        {
            SKU = "ASUS-TUF-002",
            Price = 55000000,
            Stock = 5,
            Product = product
        };
        _context.Brands.Add(brand);
        _context.Products.Add(product);
        _context.ProductVariants.AddRange(variant1, variant2);

        await _context.SaveChangesAsync();
        // Act
        var query = new GetProductVariantsQuery(
            Filter: new ProductVariantFilter(
            SearchTerm: "ASUS-TUF-001"));
        var handler = new GetProductVariantsQueryHandler(_context);

        var result = await handler.Handle(
            query,
            CancellationToken.None
        );
        // Assert
        Assert.Single(result.Items);
        Assert.Equal("ASUS-TUF-001", result.Items[0].SKU);
        Assert.DoesNotContain(
            result.Items,
            v => v.SKU == "ASUS-TUF-002");
    }

    [Fact]
    public async Task Handle_ShouldFilterVariantsByProductName()
    {
        // Arrange
        var brand = new Brand
        {
            Name = "Asus"
        };

        var product = new Product
        {
            Name = "ASUS TUF",
            Description = "Gaming Laptop",
            Brand = brand
        };

        var variant1 = new ProductVariant
        {
            SKU = "ASUS-TUF-001",
            Price = 50000000,
            Stock = 10,
            Product = product
        };

        var variant2 = new ProductVariant
        {
            SKU = "ASUS-TUF-002",
            Price = 55000000,
            Stock = 5,
            Product = product
        };

        _context.Brands.Add(brand);
        _context.Products.Add(product);
        _context.ProductVariants.AddRange(variant1, variant2);

        await _context.SaveChangesAsync();
        // Act
        var query = new GetProductVariantsQuery(
            Filter: new ProductVariantFilter(
            SearchTerm: "ASUS TUF"));

        var handler = new GetProductVariantsQueryHandler(_context);

        var result = await handler.Handle(
            query,
            CancellationToken.None
        );
        // Assert
        Assert.Equal(2, result.Items.Count);
        Assert.Contains(result.Items, v => v.SKU == "ASUS-TUF-001");
        Assert.Contains(result.Items, v => v.SKU == "ASUS-TUF-002");
    }

    [Fact]
    public async Task Handle_ShouldFilterVariantsByBrand()
    {
        //Arrange
        var asus = new Brand { Name = "Asus" };
        var lenovo = new Brand { Name = "Lenovo" };

        var asusProduct = new Product
        {
            Name = "ASUS TUF",
            Description = "Gaming Laptop",
            Brand = asus
        };

        var lenovoProduct = new Product
        {
            Name = "Lenovo Legion",
            Description = "Gaming Laptop",
            Brand = lenovo
        };

        var asusVariant = new ProductVariant
        {
            SKU = "ASUS-001",
            Price = 50000000,
            Stock = 10,
            Product = asusProduct
        };

        var lenovoVariant = new ProductVariant
        {
            SKU = "LENOVO-001",
            Price = 60000000,
            Stock = 5,
            Product = lenovoProduct
        };

        _context.Brands.AddRange(asus, lenovo);
        _context.Products.AddRange(asusProduct, lenovoProduct);
        _context.ProductVariants.AddRange(asusVariant, lenovoVariant);

        await _context.SaveChangesAsync();

        // Act
        var query = new GetProductVariantsQuery(
             Filter: new ProductVariantFilter(
            BrandId: asus.Id));

        var handler = new GetProductVariantsQueryHandler(_context);

        var result = await handler.Handle(
            query,
            CancellationToken.None
        );
        // Assert
        Assert.Single(result.Items);
        Assert.Equal("ASUS-001", result.Items[0].SKU);
    }

    [Fact]
    public async Task Handle_ShouldFilterVariantsByCategoryIncludingDescendants()
    {
        // Arrange
        var rootCategory = new Category
        {
            Name = "Laptop"
        };

        var childCategory = new Category
        {
            Name = "Gaming",
            ParentCategory = rootCategory
        };

        var brand = new Brand
        {
            Name = "Asus"
        };

        var product1 = new Product
        {
            Name = "ASUS TUF",
            Description = "Gaming Laptop",
            Brand = brand
        };

        var product2 = new Product
        {
            Name = "Lenovo Legion",
            Description = "Gaming Laptop",
            Brand = brand
        };

        var variant1 = new ProductVariant
        {
            SKU = "ASUS-001",
            Price = 50000000,
            Stock = 10,
            Product = product1
        };

        var variant2 = new ProductVariant
        {
            SKU = "LENOVO-001",
            Price = 60000000,
            Stock = 5,
            Product = product2
        };

        product1.Categories.Add(new ProductCategory
        {
            Product = product1,
            Category = childCategory
        });

        product2.Categories.Add(new ProductCategory
        {
            Product = product2,
            Category = rootCategory
        });

        _context.Brands.Add(brand);
        _context.Categories.AddRange(rootCategory, childCategory);
        _context.Products.AddRange(product1, product2);
        _context.ProductVariants.AddRange(variant1, variant2);

        await _context.SaveChangesAsync();

        // Act
        var query = new GetProductVariantsQuery(
            Filter: new ProductVariantFilter(
                CategoryId: rootCategory.Id
            )
        );

        var handler = new GetProductVariantsQueryHandler(_context);

        var result = await handler.Handle(
            query,
            CancellationToken.None
        );

        // Assert
        Assert.Equal(2, result.Items.Count);
        Assert.Contains(result.Items, v => v.SKU == "ASUS-001");
        Assert.Contains(result.Items, v => v.SKU == "LENOVO-001");
    }

    [Fact]
    public async Task Handle_ShouldSortVariantsByCheapest()
    {
        // Arrange
        var brand = new Brand
        {
            Name = "Asus"
        };

        var product = new Product
        {
            Name = "ASUS TUF",
            Description = "Gaming Laptop",
            Brand = brand
        };

        var expensiveVariant = new ProductVariant
        {
            SKU = "ASUS-EXPENSIVE",
            Price = 60000000,
            Stock = 5,
            Product = product
        };

        var cheapVariant = new ProductVariant
        {
            SKU = "ASUS-CHEAP",
            Price = 40000000,
            Stock = 10,
            Product = product
        };

        var mediumVariant = new ProductVariant
        {
            SKU = "ASUS-MEDIUM",
            Price = 50000000,
            Stock = 8,
            Product = product
        };

        _context.Brands.Add(brand);
        _context.Products.Add(product);
        _context.ProductVariants.AddRange(
            expensiveVariant,
            cheapVariant,
            mediumVariant
        );

        await _context.SaveChangesAsync();

        // Act
        var query = new GetProductVariantsQuery(
            Filter: new ProductVariantFilter(
                SortBy: ProductVariantSortBy.Cheapest
            )
        );

        var handler = new GetProductVariantsQueryHandler(_context);

        var result = await handler.Handle(
            query,
            CancellationToken.None
        );

        // Assert
        Assert.Equal(3, result.Items.Count);

        Assert.Equal("ASUS-CHEAP", result.Items[0].SKU);
        Assert.Equal("ASUS-MEDIUM", result.Items[1].SKU);
        Assert.Equal("ASUS-EXPENSIVE", result.Items[2].SKU);
    }

    [Fact]
    public async Task Handle_ShouldSortVariantsByMostExpensive()
    {
        // Arrange
        var brand = new Brand
        {
            Name = "Asus"
        };

        var product = new Product
        {
            Name = "ASUS TUF",
            Description = "Gaming Laptop",
            Brand = brand
        };

        var cheapVariant = new ProductVariant
        {
            SKU = "ASUS-CHEAP",
            Price = 40000000,
            Stock = 10,
            Product = product
        };

        var expensiveVariant = new ProductVariant
        {
            SKU = "ASUS-EXPENSIVE",
            Price = 60000000,
            Stock = 5,
            Product = product
        };

        var mediumVariant = new ProductVariant
        {
            SKU = "ASUS-MEDIUM",
            Price = 50000000,
            Stock = 8,
            Product = product
        };

        _context.Brands.Add(brand);
        _context.Products.Add(product);
        _context.ProductVariants.AddRange(
            cheapVariant,
            expensiveVariant,
            mediumVariant
        );

        await _context.SaveChangesAsync();

        // Act
        var query = new GetProductVariantsQuery(
            Filter: new ProductVariantFilter(
                SortBy: ProductVariantSortBy.MostExpensive
            )
        );

        var handler = new GetProductVariantsQueryHandler(_context);

        var result = await handler.Handle(
            query,
            CancellationToken.None
        );

        // Assert
        Assert.Equal(3, result.Items.Count);

        Assert.Equal("ASUS-EXPENSIVE", result.Items[0].SKU);
        Assert.Equal("ASUS-MEDIUM", result.Items[1].SKU);
        Assert.Equal("ASUS-CHEAP", result.Items[2].SKU);
    }

    [Fact]
    public async Task Handle_ShouldSortVariantsByNewest()
    {
        // Arrange
        var brand = new Brand
        {
            Name = "Asus"
        };

        var product = new Product
        {
            Name = "ASUS TUF",
            Description = "Gaming Laptop",
            Brand = brand
        };

        var oldVariant = new ProductVariant
        {
            SKU = "ASUS-OLD",
            Price = 50000000,
            Stock = 5,
            Product = product,
            CreatedOn = new DateTime(2026, 1, 1)
        };

        var newVariant = new ProductVariant
        {
            SKU = "ASUS-NEW",
            Price = 50000000,
            Stock = 10,
            Product = product,
            CreatedOn = new DateTime(2026, 10, 1)
        };

        var middleVariant = new ProductVariant
        {
            SKU = "ASUS-MIDDLE",
            Price = 50000000,
            Stock = 8,
            Product = product,
            CreatedOn = new DateTime(2026, 5, 1)
        };

        _context.Brands.Add(brand);
        _context.Products.Add(product);

        _context.ProductVariants.AddRange(
            oldVariant,
            newVariant,
            middleVariant
        );

        await _context.SaveChangesAsync();

        // Act
        var query = new GetProductVariantsQuery(
            Filter: new ProductVariantFilter(
                SortBy: ProductVariantSortBy.Newest
            )
        );

        var handler = new GetProductVariantsQueryHandler(_context);

        var result = await handler.Handle(
            query,
            CancellationToken.None
        );

        // Assert
        Assert.Equal(3, result.Items.Count);

        Assert.Equal("ASUS-NEW", result.Items[0].SKU);
        Assert.Equal("ASUS-MIDDLE", result.Items[1].SKU);
        Assert.Equal("ASUS-OLD", result.Items[2].SKU);
    }

    [Fact]
    public async Task Handle_ShouldReturnCorrectPage()
    {
        // Arrange
        var brand = new Brand
        {
            Name = "Asus"
        };

        var product = new Product
        {
            Name = "ASUS TUF",
            Description = "Gaming Laptop",
            Brand = brand
        };

        var variants = new List<ProductVariant>();

        for (int i = 1; i <= 5; i++)
        {
            variants.Add(new ProductVariant
            {
                SKU = $"ASUS-{i:000}",
                Price = i * 1000000,
                Stock = 10,
                Product = product
            });
        }

        _context.Brands.Add(brand);
        _context.Products.Add(product);
        _context.ProductVariants.AddRange(variants);

        await _context.SaveChangesAsync();

        // Act
        var query = new GetProductVariantsQuery(
            PageNumber: 2,
            PageSize: 2
        );

        var handler = new GetProductVariantsQueryHandler(_context);

        var result = await handler.Handle(
            query,
            CancellationToken.None
        );

        // Assert
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(5, result.TotalCount);
        Assert.Equal(2, result.PageNumber);
        Assert.Equal(2, result.PageSize);

        Assert.Equal("ASUS-003", result.Items[0].SKU);
        Assert.Equal("ASUS-004", result.Items[1].SKU);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyResult_WhenNoVariantsFound()
    {
        // Arrange
        var brand = new Brand
        {
            Name = "Asus"
        };

        var product = new Product
        {
            Name = "ASUS TUF",
            Description = "Gaming Laptop",
            Brand = brand
        };

        var variant = new ProductVariant
        {
            SKU = "ASUS-001",
            Price = 50000000,
            Stock = 10,
            Product = product
        };

        _context.Brands.Add(brand);
        _context.Products.Add(product);
        _context.ProductVariants.Add(variant);

        await _context.SaveChangesAsync();

        // Act
        var query = new GetProductVariantsQuery(
            Filter: new ProductVariantFilter(
                SearchTerm: "NOT-EXISTING-SKU"
            )
        );

        var handler = new GetProductVariantsQueryHandler(_context);

        var result = await handler.Handle(
            query,
            CancellationToken.None
        );

        // Assert
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
    }

    [Fact]
    public async Task Handle_ShouldReturnOneVariant_WhenPageSizeIsOne()
    {
        // Arrange
        var brand = new Brand
        {
            Name = "Asus"
        };

        var product = new Product
        {
            Name = "ASUS TUF",
            Description = "Gaming Laptop",
            Brand = brand
        };

        var variant1 = new ProductVariant
        {
            SKU = "ASUS-001",
            Price = 1000000,
            Stock = 10,
            Product = product
        };

        var variant2 = new ProductVariant
        {
            SKU = "ASUS-002",
            Price = 2000000,
            Stock = 10,
            Product = product
        };

        _context.Brands.Add(brand);
        _context.Products.Add(product);
        _context.ProductVariants.AddRange(variant1, variant2);

        await _context.SaveChangesAsync();

        // Act
        var query = new GetProductVariantsQuery(
            PageNumber: 1,
            PageSize: 1
        );

        var handler = new GetProductVariantsQueryHandler(_context);

        var result = await handler.Handle(
            query,
            CancellationToken.None
        );

        // Assert
        Assert.Single(result.Items);
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(1, result.PageSize);
        Assert.Equal(1, result.PageNumber);
    }

    [Fact]
    public async Task Handle_ShouldAcceptMaximumPageSize()
    {
        // Arrange
        var brand = new Brand
        {
            Name = "Asus"
        };

        var product = new Product
        {
            Name = "ASUS TUF",
            Description = "Gaming Laptop",
            Brand = brand
        };

        var variants = new List<ProductVariant>();

        for (int i = 1; i <= 100; i++)
        {
            variants.Add(new ProductVariant
            {
                SKU = $"ASUS-{i:000}",
                Price = i * 1000000,
                Stock = 10,
                Product = product
            });
        }

        _context.Brands.Add(brand);
        _context.Products.Add(product);
        _context.ProductVariants.AddRange(variants);

        await _context.SaveChangesAsync();

        // Act
        var query = new GetProductVariantsQuery(
            PageNumber: 1,
            PageSize: 100
        );

        var handler = new GetProductVariantsQueryHandler(_context);

        var result = await handler.Handle(
            query,
            CancellationToken.None
        );

        // Assert
        Assert.Equal(100, result.Items.Count);
        Assert.Equal(100, result.TotalCount);
        Assert.Equal(100, result.PageSize);
        Assert.Equal(1, result.PageNumber);
    }

    [Fact]
    public async Task Handle_ShouldSortById_WhenPricesAreEqual()
    {
        // Arrange
        var brand = new Brand { Name = "Asus" };

        var product = new Product
        {
            Name = "ASUS TUF",
            Description = "Gaming Laptop",
            Brand = brand
        };

        var variant1 = new ProductVariant
        {
            SKU = "ASUS-003",
            Price = 50000000,
            Stock = 10,
            Product = product
        };

        var variant2 = new ProductVariant
        {
            SKU = "ASUS-001",
            Price = 50000000,
            Stock = 10,
            Product = product
        };

        var variant3 = new ProductVariant
        {
            SKU = "ASUS-002",
            Price = 50000000,
            Stock = 10,
            Product = product
        };

        _context.Brands.Add(brand);
        _context.Products.Add(product);

        _context.ProductVariants.AddRange(
            variant1,
            variant2,
            variant3
        );

        await _context.SaveChangesAsync();

        // Act
        var query = new GetProductVariantsQuery(
            Filter: new ProductVariantFilter(
                SortBy: ProductVariantSortBy.Cheapest
            )
        );

        var handler = new GetProductVariantsQueryHandler(_context);

        var result = await handler.Handle(
            query,
            CancellationToken.None
        );

        // Assert
        Assert.Equal(3, result.Items.Count);

        Assert.Equal("ASUS-003", result.Items[0].SKU);
        Assert.Equal("ASUS-001", result.Items[1].SKU);
        Assert.Equal("ASUS-002", result.Items[2].SKU);
    }
    public void Dispose()
    {
        _context.Dispose();
    }
}
