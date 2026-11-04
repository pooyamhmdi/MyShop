using MediatR;
using OnlineShop.Application.Common.Models;
using OnlineShop.Application.Features.Products.Dtos;

namespace OnlineShop.Application.Features.Products.Queries.GetProductVariants;

public record GetProductVariantsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    ProductVariantFilter? Filter = null
) : IRequest<PagedResult<ProductVariantDto>>;