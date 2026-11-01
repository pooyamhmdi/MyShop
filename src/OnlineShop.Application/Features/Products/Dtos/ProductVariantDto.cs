namespace OnlineShop.Application.Features.Products.Dtos;

public record ProductVariantDto(
    long Id,
    string SKU,
    decimal Price,
    int Stock,
    IReadOnlyList<ProductImageDto> Images
);
