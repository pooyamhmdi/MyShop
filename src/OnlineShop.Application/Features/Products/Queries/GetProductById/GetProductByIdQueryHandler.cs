using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Common.Interfaces;
using OnlineShop.Application.Features.Products.Dtos;

namespace OnlineShop.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler
    : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IApplicationDbContext _context;

    public GetProductByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDto> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Where(p => p.Id == request.ProductId)
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Description,
                p.BrandId,

                // Product Images
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

                // Product Variants
                p.Variants
                    .OrderBy(v => v.Id)
                    .Select(v => new ProductVariantDto(
                        v.Id,
                        v.ProductId,
                        v.Product.Name,
                        v.SKU,
                        v.Price,
                        v.Stock,

                        // Variant Images
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
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null)
        {
            throw new KeyNotFoundException(
                $"Product with ID {request.ProductId} was not found.");
        }

        return product;
    }
}