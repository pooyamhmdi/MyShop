using FluentValidation;

namespace OnlineShop.Application.Features.Products.Queries.GetProductVariantById.Validators;

public class GetProductVariantByIdQueryValidator
    : AbstractValidator<GetProductVariantByIdQuery>
{
    public GetProductVariantByIdQueryValidator()
    {
        RuleFor(x => x.VariantId)
            .GreaterThan(0);
    }
}