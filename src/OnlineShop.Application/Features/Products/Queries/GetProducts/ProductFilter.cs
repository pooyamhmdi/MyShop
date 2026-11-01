namespace OnlineShop.Application.Features.Products.Queries.GetProducts;

public record ProductFilter(
    string? SearchTerm = null,
    long? BrandId = null,
    long? CategoryId = null,
    ProductSortBy? SortBy = null
);
