
using FluentValidation;

namespace OnlineShop.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator()
    {
        RuleFor(query => query.ProductId)
            .GreaterThan(0)
            .WithMessage("Product ID must be greater than 0.");
    }
}
