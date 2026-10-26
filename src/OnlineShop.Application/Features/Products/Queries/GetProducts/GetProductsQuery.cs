using MediatR;
using OnlineShop.Application.Common.Models;

namespace OnlineShop.Application.Features.Products.Queries
{
    public record GetProductsQuery(int PageNumber = 1, int PageSize = 10) 
    : IRequest<PagedResult<ProductDto>>;
}