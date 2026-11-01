using OnlineShop.Application.Features.Products.Dtos;
public record ProductDto(
    long Id,
    string Name,
    string Description,
    long BrandId,
    IReadOnlyList<ProductImageDto> Images,
    IReadOnlyList<ProductVariantDto> Variants
);