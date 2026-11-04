using OnlineShop.Application.Features.Products.Queries.GetProductVariants;
using OnlineShop.Application.Features.Products.Queries.GetProductVariants.Validators;
using FluentValidation;

namespace OnlineShop.Api.Tests.Features.Products.Queries.GetProductVariants.Validators;

public class GetProductVariantsQueryValidatorTests
{
    private readonly GetProductVariantsQueryValidator _validator;

    public GetProductVariantsQueryValidatorTests()
    {
        var filterValidator = new ProductVariantFilterValidator();

        _validator = new GetProductVariantsQueryValidator(filterValidator);
    }

    [Fact]
    public void Validate_ShouldPass_WhenQueryIsValid()
    {
        // Arrange
        var query = new GetProductVariantsQuery(
            PageNumber: 1,
            PageSize: 10
        );

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldFail_WhenPageNumberIsZero()
    {
        // Arrange
        var query = new GetProductVariantsQuery(
            PageNumber: 0,
            PageSize: 10
        );

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldFail_WhenPageSizeIsZero()
    {
        // Arrange
        var query = new GetProductVariantsQuery(
            PageNumber: 1,
            PageSize: 0
        );

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldFail_WhenPageSizeExceedsMaximum()
    {
        // Arrange
        var query = new GetProductVariantsQuery(
            PageNumber: 1,
            PageSize: 101
        );

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldFail_WhenFilterIsInvalid()
    {
        // Arrange
        var query = new GetProductVariantsQuery(
            Filter: new ProductVariantFilter(
                BrandId: 0
            )
        );

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
    }
    [Fact]
    public void Validate_ShouldFail_WhenPageNumberIsNegative()
    {
        // Arrange
        var query = new GetProductVariantsQuery(
            PageNumber: -1,
            PageSize: 10
        );

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
    }
    [Fact]
    public void Validate_ShouldFail_WhenPageSizeIsNegative()
    {
        // Arrange
        var query = new GetProductVariantsQuery(
            PageNumber: 1,
            PageSize: -1
        );

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
    }
    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    public void Validate_ShouldPass_WhenPageSizeIsWithinAllowedRange(int pageSize)
    {
        // Arrange
        var query = new GetProductVariantsQuery(
            PageSize: pageSize
        );

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.True(result.IsValid);
    }
    [Fact]
    public void Validate_ShouldPass_WhenFilterIsNull()
    {
        // Arrange
        var query = new GetProductVariantsQuery(
            Filter: null
        );

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.True(result.IsValid);
    }
}