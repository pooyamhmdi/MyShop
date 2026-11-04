using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Common.Interfaces;
using OnlineShop.Application.Common.Models;
using OnlineShop.Application.Common.Extensions;
using OnlineShop.Domain.Entities.product;
using OnlineShop.Application.Features.Products.Dtos;

namespace OnlineShop.Application.Features.Products.Queries.GetProductVariants;

public class GetProductVariantsQueryHandler
    : IRequestHandler<GetProductVariantsQuery, PagedResult<ProductVariantDto>>
{
    private readonly IApplicationDbContext _context;

    public GetProductVariantsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<ProductVariantDto>> Handle(
        GetProductVariantsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.ProductVariants
            .AsNoTracking();

        query = ApplySearchFilter(query, request.Filter?.SearchTerm);

        query = ApplyBrandFilter(query, request.Filter?.BrandId);

        query = await ApplyCategoryFilter(query, request.Filter?.CategoryId, cancellationToken);

        query = ApplySorting(query, request.Filter?.SortBy);

        var result = query.Select(v => new ProductVariantDto(
            v.Id,
            v.ProductId,
            v.Product.Name,
            v.SKU,
            v.Price,
            v.Stock,

            v.Images
                .OrderByDescending(i => i.IsMain)
                .ThenBy(i => i.DisplayOrder)
                .ThenBy(i => i.Id)
                .Select(i => new ProductImageDto(
                    i.Id,
                    i.ImageUrl,
                    i.IsMain,
                    i.DisplayOrder
                ))
                .ToList()
        ));

        return await result.PaginateAsync(
            request.PageNumber,
            request.PageSize,
            cancellationToken
        );
    }

    private IQueryable<ProductVariant> ApplySearchFilter(
        IQueryable<ProductVariant> query,
        string? searchTerm)
    {
        var normalizedSearchTerm = searchTerm?.Trim();

        if (!string.IsNullOrWhiteSpace(normalizedSearchTerm))
        {
            query = query.Where(v =>
                v.SKU.Contains(normalizedSearchTerm) ||
                v.Product.Name.Contains(normalizedSearchTerm));
        }

        return query;
    }

    private IQueryable<ProductVariant> ApplyBrandFilter(
        IQueryable<ProductVariant> query,
        long? brandId)
    {
        if (brandId.HasValue)
        {
            query = query.Where(v =>
                v.Product.BrandId == brandId.Value);
        }

        return query;
    }

    private async Task<IQueryable<ProductVariant>> ApplyCategoryFilter(
        IQueryable<ProductVariant> query,
        long? categoryId,
        CancellationToken cancellationToken)
    {
        if (!categoryId.HasValue)
            return query;

        var categoryIds = new List<long> { categoryId.Value };
        var currentLevelIds = new List<long> { categoryId.Value };

        // Get descendants up to four levels.
        for (int level = 0; level < 4; level++)
        {
            var childIds = await _context.Categories
                .AsNoTracking()
                .Where(c =>
                    c.ParentCategoryId.HasValue &&
                    currentLevelIds.Contains(c.ParentCategoryId.Value))
                .Select(c => c.Id)
                .ToListAsync(cancellationToken);

            if (childIds.Count == 0)
                break;

            categoryIds.AddRange(childIds);
            currentLevelIds = childIds;
        }

        return query.Where(v =>
            v.Product.Categories.Any(c =>
                categoryIds.Contains(c.CategoryId)));
    }

    private IQueryable<ProductVariant> ApplySorting(
        IQueryable<ProductVariant> query,
        ProductVariantSortBy? sortBy)
    {
        return sortBy switch
        {
            ProductVariantSortBy.Cheapest =>
                query.OrderBy(v => v.Price)
                     .ThenBy(v => v.Id),

            ProductVariantSortBy.MostExpensive =>
                query.OrderByDescending(v => v.Price)
                     .ThenBy(v => v.Id),

            ProductVariantSortBy.Newest =>
                query.OrderByDescending(v => v.CreatedOn)
                     .ThenBy(v => v.Id),

            _ => query.OrderBy(v => v.Id)
        };
    }
}
