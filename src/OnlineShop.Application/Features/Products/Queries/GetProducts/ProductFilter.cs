namespace OnlineShop.Application.Features.Products.Queries;

public record ProductFilter(
    string? SearchTerm = null,
    long? BrandId = null,
    long? CategoryId = null,
    ProductSortBy? SortBy = null,
    bool SortDescending = false
);
