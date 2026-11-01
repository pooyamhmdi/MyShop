using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Common.Interfaces;
using OnlineShop.Application.Common.Models;
using OnlineShop.Application.Common.Extensions;
using OnlineShop.Domain.Entities.product;
using OnlineShop.Application.Features.Products.Dtos;

namespace OnlineShop.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler
    : IRequestHandler<GetProductsQuery, PagedResult<ProductDto>>
{
    private readonly IApplicationDbContext _context;

    public GetProductsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<ProductDto>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Products.AsNoTracking();

        query = ApplySearchFilter(query, request.Filter?.SearchTerm);

        query = ApplyBrandFilter(query, request.Filter?.BrandId);

        query = await ApplyCategoryFilter(query, request.Filter?.CategoryId, cancellationToken);

        query = ApplyProductSorting(query, request.Filter?.SortBy);

        var products = query.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Description,
            p.BrandId,

            p.Images
                .OrderByDescending(i => i.IsMain)
                .ThenBy(i => i.DisplayOrder)
                .ThenBy(i => i.Id)
                .Select(i => new ProductImageDto(
                    i.Id,
                    i.ImageUrl,
                    i.IsMain,
                    i.DisplayOrder
                ))
                .ToList(),

            p.Variants
                .OrderBy(v => v.Id)
                .Select(v => new ProductVariantDto(
                    v.Id,
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
                ))
                .ToList()
        )); ;

        return await products.PaginateAsync(
            request.PageNumber,
            request.PageSize,
            cancellationToken);
    }

    private IQueryable<Product> ApplySearchFilter(
        IQueryable<Product> query,
        string? searchTerm)
    {
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.Trim();

            query = query.Where(p =>
                p.Name.Contains(searchTerm) ||
                p.Brand.Name.Contains(searchTerm));
        }

        return query;
    }

    private IQueryable<Product> ApplyBrandFilter(
        IQueryable<Product> query,
        long? brandId)
    {
        if (brandId.HasValue)
        {
            query = query.Where(p =>
                p.BrandId == brandId.Value);
        }

        return query;
    }

    private async Task<IQueryable<Product>> ApplyCategoryFilter(
        IQueryable<Product> query,
        long? categoryId,
        CancellationToken cancellationToken)
    {
        if (!categoryId.HasValue)
            return query;

        var categoryIds = new List<long>
        {
            categoryId.Value
        };

        var currentLevelIds = new List<long>
        {
            categoryId.Value
        };

        // چهار سطح فرزند به‌علاوه دسته اصلی
        for (int i = 0; i < 4; i++)
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

        return query.Where(p =>
            p.Categories.Any(c =>
                categoryIds.Contains(c.CategoryId)));
    }

    private IQueryable<Product> ApplyProductSorting(
        IQueryable<Product> query,
        ProductSortBy? sortBy)
    {
        return sortBy switch
        {
            ProductSortBy.Name =>
                query.OrderBy(p => p.Name)
                     .ThenBy(p => p.Id),

            ProductSortBy.Newest =>
                query.OrderByDescending(p => p.CreatedOn)
                     .ThenBy(p => p.Id),

            ProductSortBy.Oldest =>
                query.OrderBy(p => p.CreatedOn)
                     .ThenBy(p => p.Id),

            _ =>
                query.OrderBy(p => p.Id)
        };
    }
}


