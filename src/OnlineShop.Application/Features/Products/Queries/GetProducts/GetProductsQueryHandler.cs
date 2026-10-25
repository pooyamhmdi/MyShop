using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Common.Extensions;
using OnlineShop.Application.Common.Interfaces;
using OnlineShop.Application.Common.Models;
using OnlineShop.Application.Features.Products.Dtos;

namespace OnlineShop.Application.Features.Products.Queries;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedResult<ProductDto>>
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
        var query = _context.Products
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Description,
                p.BrandId,
                p.Variants
                    .Select(v => new ProductVariantDto(
                        v.Id,
                        v.SKU,
                        v.Price,
                        v.Stock))
                    .ToList()
            ));

        return await query.PaginateAsync(
            request.PageNumber,
            request.PageSize,
            cancellationToken);
    }
}
