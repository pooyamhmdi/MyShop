using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Common.Interfaces;
using OnlineShop.Application.Common.Models;
using OnlineShop.Application.Common.Extensions;
using OnlineShop.Application.Features.Products;
using OnlineShop.Application.Features.Products.Queries;
using OnlineShop.Domain.Entities.product;

namespace OnlineShop.Application.Features.Products.Queries;

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
        query = ApplyCategoryFilter(query, request.Filter?.CategoryId);
        query = ApplySorting(query, request.Filter);

        var products = query.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Description,
            p.BrandId,
            p.Variants.Select(v => new ProductVariantDto(
                v.Id,
                v.SKU,
                v.Price,
                v.Stock
            )).ToList()
        ));

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
            query = query.Where(p => p.BrandId == brandId.Value);
        }

        return query;
    }

    private IQueryable<Product> ApplyCategoryFilter(
        IQueryable<Product> query,
        long? categoryId)
    {
        if (categoryId.HasValue)
        {
            query = query.Where(p =>
                p.Categories.Any(c => c.CategoryId == categoryId.Value));
        }

        return query;
    }

    private IQueryable<Product> ApplySorting(
        IQueryable<Product> query,
        ProductFilter? filter)
    {
        if (filter is null || filter.SortBy is null)
        {
            return query.OrderBy(p => p.Id);
        }

        switch (filter.SortBy.Value)
        {
            case ProductSortBy.Name:
                query = filter.SortDescending
                    ? query.OrderByDescending(p => p.Name).ThenBy(p => p.Id)
                    : query.OrderBy(p => p.Name).ThenBy(p => p.Id);
                break;

            case ProductSortBy.Newest:
                query = filter.SortDescending
                    ? query.OrderByDescending(p => p.CreatedOn).ThenBy(p => p.Id)
                    : query.OrderBy(p => p.CreatedOn).ThenBy(p => p.Id);
                break;

            case ProductSortBy.Price:
                query = query.OrderBy(p => p.Variants.Any() ? 0 : 1);

                if (filter.SortDescending)
                {
                    query = ((IOrderedQueryable<Product>)query)
                        .ThenByDescending(
                            p => p.Variants.Min(v => (decimal?)v.Price));
                }
                else
                {
                    query = ((IOrderedQueryable<Product>)query)
                        .ThenBy(
                            p => p.Variants.Min(v => (decimal?)v.Price));
                }

                query = ((IOrderedQueryable<Product>)query)
                    .ThenBy(p => p.Id);
                break;

            default:
                query = query.OrderBy(p => p.Id);
                break;
        }

        return query;
    }
}


