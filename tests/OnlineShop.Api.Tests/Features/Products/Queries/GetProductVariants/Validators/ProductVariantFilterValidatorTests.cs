
using OnlineShop.Application.Features.Products.Queries.GetProductVariants;
using OnlineShop.Application.Features.Products.Queries.GetProductVariants.Validators;

namespace OnlineShop.Api.Tests.Features.Products.Queries.GetProductVariants.Validators;

public class ProductVariantFilterValidatorTests
{
    private readonly ProductVariantFilterValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_WhenFilterIsValid()
    {
        // Arrange
        var filter = new ProductVariantFilter(
            SearchTerm: "ASUS TUF",
            BrandId: 1,
            CategoryId: 2,
            SortBy: ProductVariantSortBy.Cheapest
        );

        // Act
        var result = _validator.Validate(filter);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldFail_WhenSearchTermExceedsMaximumLength()
    {
        // Arrange
        var filter = new ProductVariantFilter(
            SearchTerm: new string('A', 101)
        );

        // Act
        var result = _validator.Validate(filter);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error =>
            error.PropertyName == nameof(ProductVariantFilter.SearchTerm));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_ShouldFail_WhenBrandIdIsInvalid(long brandId)
    {
        // Arrange
        var filter = new ProductVariantFilter(BrandId: brandId);

        // Act
        var result = _validator.Validate(filter);

        // Assert
        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_ShouldFail_WhenCategoryIdIsInvalid(long categoryId)
    {
        // Arrange
        var filter = new ProductVariantFilter(CategoryId: categoryId);

        // Act
        var result = _validator.Validate(filter);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldFail_WhenSortByIsInvalid()
    {
        // Arrange
        var filter = new ProductVariantFilter(
            SortBy: (ProductVariantSortBy)999
        );

        // Act
        var result = _validator.Validate(filter);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldPass_WhenOptionalPropertiesAreNull()
    {
        // Arrange
        var filter = new ProductVariantFilter();

        // Act
        var result = _validator.Validate(filter);

        // Assert
        Assert.True(result.IsValid);
    }
}
