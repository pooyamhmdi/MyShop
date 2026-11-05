using FluentValidation.TestHelper;
using OnlineShop.Application.Features.Products.Queries.GetProductVariantById;
using OnlineShop.Application.Features.Products.Queries.GetProductVariantById.Validators;

namespace OnlineShop.Api.Tests.Features.Products.Queries.GetProductVariantById.Validators;

public class GetProductVariantByIdQueryValidatorTests
{
    private readonly GetProductVariantByIdQueryValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_WhenVariantIdIsPositive()
    {
        var query = new GetProductVariantByIdQuery(1);

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveValidationErrorFor(x => x.VariantId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_ShouldFail_WhenVariantIdIsNotPositive(long variantId)
    {
        var query = new GetProductVariantByIdQuery(variantId);

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.VariantId);
    }
}
