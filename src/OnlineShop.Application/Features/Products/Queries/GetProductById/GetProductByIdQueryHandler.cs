using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Common.Interfaces;

namespace OnlineShop.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IApplicationDbContext _context;
        public GetProductByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
            .AsNoTracking()
            .Where(p => p.Id == request.ProductId)
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
                     )).FirstOrDefaultAsync(cancellationToken);

            if (product is null)
            {
                throw new KeyNotFoundException(
                    $"Product with ID {request.ProductId} was not found.");
            }

            return product;
        }
    }
}