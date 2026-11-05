using MediatR;
using OnlineShop.Application.Features.Products.Dtos;


namespace OnlineShop.Application.Features.Products.Queries.GetProductVariantById;

public record GetProductVariantByIdQuery(long VariantId)
    : IRequest<ProductVariantDto>;