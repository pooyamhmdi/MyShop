using MediatR;
using OnlineShop.Application.Features.Products;

namespace OnlineShop.Application.Features.Products.Queries.GetProductById;

public record GetProductByIdQuery(long ProductId)
    : IRequest<ProductDto>;
