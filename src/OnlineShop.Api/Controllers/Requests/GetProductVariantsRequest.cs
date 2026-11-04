using OnlineShop.Application.Features.Products.Queries.GetProductVariants;
namespace OnlineShop.Api.Controllers.Requests;

public record GetProductVariantsRequest
{
    public int PageNumber { get; init; } = 1;

    public int PageSize { get; init; } = 20;

    public string? SearchTerm { get; init; }

    public long? BrandId { get; init; }

    public long? CategoryId { get; init; }

    public ProductVariantSortBy? SortBy { get; init; }
}