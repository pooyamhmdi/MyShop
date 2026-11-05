using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities.brand;
using OnlineShop.Domain.Entities.product;
using OnlineShop.Infrastracture.Persistence;
using OnlineShop.Application.Features.Products.Queries.GetProductVariantById;

namespace OnlineShop.Api.Tests.Features.Products.Queries.GetProductVariantById.Handlers;

public class GetProductVariantByIdQueryHandlerTests : IDisposable
{
    private readonly OnlineShopDbContext _context;

    public GetProductVariantByIdQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<OnlineShopDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new OnlineShopDbContext(options);
    }
    [Fact]
    public async Task Handle_ShouldReturnVariantWithProductAndVariantImages()
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
            SKU = "ASUS-TUF-001",
            Price = 50000000,
            Stock = 10,
            Product = product
        };

        var productImage = new ProductImage
        {
            ImageUrl = "product-image.jpg",
            IsMain = true,
            DisplayOrder = 1,
            Product = product
        };

        var variantImage = new ProductImage
        {
            ImageUrl = "variant-image.jpg",
            IsMain = false,
            DisplayOrder = 2,
            ProductVariant = variant
        };

        _context.Brands.Add(brand);
        _context.Products.Add(product);
        _context.ProductVariants.Add(variant);
        _context.ProductImages.AddRange(productImage, variantImage);

        await _context.SaveChangesAsync();

        // Act
        var handler = new GetProductVariantByIdQueryHandler(_context);

        var result = await handler.Handle(
            new GetProductVariantByIdQuery(variant.Id),
            CancellationToken.None
        );

        // Assert
        Assert.Equal(variant.Id, result.Id);
        Assert.Equal(product.Id, result.ProductId);
        Assert.Equal(product.Name, result.ProductName);
        Assert.Equal(variant.SKU, result.SKU);

        Assert.Equal(2, result.Images.Count);
        Assert.Equal("product-image.jpg", result.Images[0].ImageUrl);
        Assert.Equal("variant-image.jpg", result.Images[1].ImageUrl);
    }
    [Fact]
    public async Task Handle_ShouldReturnImagesInCorrectOrder()
    {
        // Arrange
        var brand = new Brand { Name = "Asus" };

        var product = new Product
        {
            Name = "ASUS TUF",
            Description = "Gaming Laptop",
            Brand = brand
        };

        var variant = new ProductVariant
        {
            SKU = "ASUS-TUF-001",
            Price = 50000000,
            Stock = 10,
            Product = product
        };

        var image1 = new ProductImage
        {
            ImageUrl = "image-1.jpg",
            IsMain = false,
            DisplayOrder = 1,
            ProductVariant = variant
        };

        var image2 = new ProductImage
        {
            ImageUrl = "image-2.jpg",
            IsMain = true,
            DisplayOrder = 3,
            ProductVariant = variant
        };

        var image3 = new ProductImage
        {
            ImageUrl = "image-3.jpg",
            IsMain = false,
            DisplayOrder = 2,
            ProductVariant = variant
        };

        var image4 = new ProductImage
        {
            ImageUrl = "image-4.jpg",
            IsMain = true,
            DisplayOrder = 1,
            ProductVariant = variant
        };

        _context.Brands.Add(brand);
        _context.Products.Add(product);
        _context.ProductVariants.Add(variant);
        _context.ProductImages.AddRange(image1, image2, image3, image4);

        await _context.SaveChangesAsync();

        // Act
        var handler = new GetProductVariantByIdQueryHandler(_context);

        var result = await handler.Handle(
            new GetProductVariantByIdQuery(variant.Id),
            CancellationToken.None
        );

        // Assert
        Assert.Equal(4, result.Images.Count);

        Assert.Equal("image-4.jpg", result.Images[0].ImageUrl);
        Assert.Equal("image-2.jpg", result.Images[1].ImageUrl);
        Assert.Equal("image-1.jpg", result.Images[2].ImageUrl);
        Assert.Equal("image-3.jpg", result.Images[3].ImageUrl);
    }
    [Fact]
    public async Task Handle_ShouldThrowKeyNotFoundException_WhenVariantDoesNotExist()
    {
        // Arrange
        var handler = new GetProductVariantByIdQueryHandler(_context);

        var query = new GetProductVariantByIdQuery(999);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => handler.Handle(query, CancellationToken.None)
        );
    }
    public void Dispose()
    {
        _context.Dispose();
    }
}