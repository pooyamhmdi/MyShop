namespace OnlineShop.Application.Features.Products.Queries.GetProductVariants;

public record ProductVariantFilter(
    string? SearchTerm = null,
    long? BrandId = null,
    long? CategoryId = null,
    ProductVariantSortBy? SortBy = null
);