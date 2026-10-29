
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Features.Products.Queries;
using OnlineShop.Infrastracture.Persistence;
using OnlineShop.Domain.Entities.brand;
using OnlineShop.Domain.Entities.product;

namespace OnlineShop.Api.Tests;

public class GetProductsQueryHandlerTests
{
    [Fact]
    public async Task Handle_SearchTerm_ReturnsMatchingProducts()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<OnlineShopDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new OnlineShopDbContext(options);

        var asus = new Brand { Id = 1, Name = "ASUS" };
        var dell = new Brand { Id = 2, Name = "Dell" };

        context.Brands.AddRange(asus, dell);

        context.Products.AddRange(
            new Product
            {
                Id = 1,
                Name = "ROG Laptop",
                BrandId = 1,
                Brand = asus
            },
            new Product
            {
                Id = 2,
                Name = "Inspiron Laptop",
                BrandId = 2,
                Brand = dell
            });

        await context.SaveChangesAsync();

        var handler = new GetProductsQueryHandler(context);
        var request = new GetProductsQuery(
            Filter: new ProductFilter(SearchTerm: "ASUS"));

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        var product = Assert.Single(result.Items);
        Assert.Equal("ROG Laptop", product.Name);
    }


[Fact]
public async Task Handle_BrandIdFilter_ReturnsOnlyProductsOfSelectedBrand()
{
    // Arrange
    var options = new DbContextOptionsBuilder<OnlineShopDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;

    await using var context = new OnlineShopDbContext(options);

    var asus = new Brand { Id = 1, Name = "ASUS" };
    var dell = new Brand { Id = 2, Name = "Dell" };

    context.Brands.AddRange(asus, dell);

    context.Products.AddRange(
        new Product
        {
            Id = 1,
            Name = "ROG Laptop",
            BrandId = 1,
            Brand = asus
        },
        new Product
        {
            Id = 2,
            Name = "Inspiron Laptop",
            BrandId = 2,
            Brand = dell
        });

    await context.SaveChangesAsync();

    var handler = new GetProductsQueryHandler(context);
    var request = new GetProductsQuery(
        Filter: new ProductFilter(BrandId: 1));

    // Act
    var result = await handler.Handle(request, CancellationToken.None);

    // Assert
    var product = Assert.Single(result.Items);
    Assert.Equal("ROG Laptop", product.Name);
    Assert.Equal(1L, product.BrandId);
}
}
