using FluentValidation;

namespace OnlineShop.Application.Features.Products.Queries.GetProductVariants.Validators;

public class GetProductVariantsQueryValidator
    : AbstractValidator<GetProductVariantsQuery>
{
    public GetProductVariantsQueryValidator(
        IValidator<ProductVariantFilter> filterValidator)
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.Filter)
            .SetValidator(filterValidator!)
            .When(x => x.Filter is not null);
    }
}