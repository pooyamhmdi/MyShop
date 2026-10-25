namespace OnlineShop.Application.Features.Products.Dtos;

public record ProductDto(
    long Id,
    string Name,
    string Description,
    long BrandId,
    IReadOnlyList<ProductVariantDto> Variants
);

public record ProductVariantDto(
    long Id,
    string SKU,
    decimal Price,
    int Stock
);
