using FluentValidation;

namespace OnlineShop.Application.Features.Products.Queries.GetProductVariants.Validators;

public class ProductVariantFilterValidator
    : AbstractValidator<ProductVariantFilter>
{
    public ProductVariantFilterValidator()
    {
        RuleFor(x => x.SearchTerm)
            .MaximumLength(100);

        RuleFor(x => x.BrandId)
            .GreaterThan(0)
            .When(x => x.BrandId.HasValue);

        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .When(x => x.CategoryId.HasValue);

        RuleFor(x => x.SortBy)
            .IsInEnum()
            .When(x => x.SortBy.HasValue);
    }
}