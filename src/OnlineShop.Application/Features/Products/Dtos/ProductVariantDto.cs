namespace OnlineShop.Application.Features.Products.Dtos;

public record ProductVariantDto(
    long Id,
    long ProductId,
    string ProductName,
    string SKU,
    decimal Price,
    int Stock,
    IReadOnlyList<ProductImageDto> Images
);