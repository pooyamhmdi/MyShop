using FluentValidation;

namespace OnlineShop.Application.Features.Products.Queries;

public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    public GetProductsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");

        When(x => x.Filter is not null, () =>
        {
            RuleFor(x => x.Filter!.BrandId)
                .GreaterThan(0)
                .When(x => x.Filter!.BrandId.HasValue)
                .WithMessage("Brand ID must be greater than 0.");

            RuleFor(x => x.Filter!.CategoryId)
                .GreaterThan(0)
                .When(x => x.Filter!.CategoryId.HasValue)
                .WithMessage("Category ID must be greater than 0.");

            RuleFor(x => x.Filter!.SearchTerm)
                .Must(term => term is null || term.Trim().Length <= 100)
                .WithMessage("Search term cannot exceed 100 characters.");

            RuleFor(x => x.Filter!.SortBy)
                .IsInEnum()
                .When(x => x.Filter!.SortBy.HasValue)
                .WithMessage("Invalid product sort option.");
        });
    }
}
