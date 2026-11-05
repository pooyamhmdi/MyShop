using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Common.Interfaces;
using OnlineShop.Application.Features.Products.Dtos;

namespace OnlineShop.Application.Features.Products.Queries.GetProductVariantById;

public class GetProductVariantByIdQueryHandler
    : IRequestHandler<GetProductVariantByIdQuery, ProductVariantDto>
{
    private readonly IApplicationDbContext _context;

    public GetProductVariantByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductVariantDto> Handle(
        GetProductVariantByIdQuery request,
        CancellationToken cancellationToken)
    {
        var variant = await _context.ProductVariants
            .AsNoTracking()
            .Include(v => v.Product)
                .ThenInclude(p => p.Images)
            .Include(v => v.Images)
            .FirstOrDefaultAsync(
                v => v.Id == request.VariantId,
                cancellationToken);

        if (variant is null)
            throw new KeyNotFoundException(
                $"Product variant with ID {request.VariantId} was not found.");

        var images = variant.Product.Images
                 .Concat(variant.Images)
                 .OrderByDescending(i => i.IsMain)
                 .ThenBy(i => i.DisplayOrder)
                .ThenBy(i => i.Id)
                .Select(i => new ProductImageDto(
                    i.Id,
                    i.ImageUrl,
                    i.IsMain,
                    i.DisplayOrder)).ToList();
        return new ProductVariantDto(
                variant.Id,
                variant.ProductId,
                variant.Product.Name,
                variant.SKU,
                variant.Price,
                variant.Stock,
                images);
    }
}