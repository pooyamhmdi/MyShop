using MediatR;
using OnlineShop.Application.Common.Models;

namespace OnlineShop.Application.Features.Products.Queries.GetProducts;

public record GetProductsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    ProductFilter? Filter = null
) : IRequest<PagedResult<ProductDto>>;