using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using OnlineShop.Api.Exceptions;

namespace OnlineShop.Api.Tests;

public class GlobalExceptionHandlerTests
{
    [Fact]
    public async Task TryHandleAsync_ValidationException_Returns400()
    {
        // Arrange
        var handler = new GlobalExceptionHandler(
            NullLogger<GlobalExceptionHandler>.Instance);

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var exception = new ValidationException(
        [
            new FluentValidation.Results.ValidationFailure(
                "PageNumber",
                "Page number must be at least 1.")
        ]);

        // Act
        var handled = await handler.TryHandleAsync(
            context,
            exception,
            CancellationToken.None);

        // Assert
        Assert.True(handled);
        Assert.Equal(
            StatusCodes.Status400BadRequest,
            context.Response.StatusCode);

        context.Response.Body.Position = 0;

        using var json = await JsonDocument.ParseAsync(
            context.Response.Body);

        Assert.Equal(
            400,
            json.RootElement.GetProperty("status").GetInt32());

        Assert.Contains(
            "PageNumber",
            json.RootElement.GetProperty("errors").ToString());
    }

[Fact]
public async Task TryHandleAsync_UnexpectedException_Returns500WithoutLeakingDetails()
{
    // Arrange
    var handler = new GlobalExceptionHandler(
        NullLogger<GlobalExceptionHandler>.Instance);

    var context = new DefaultHttpContext();
    context.Response.Body = new MemoryStream();

    var exception = new InvalidOperationException(
        "Sensitive internal database details.");

    // Act
    var handled = await handler.TryHandleAsync(
        context,
        exception,
        CancellationToken.None);

    // Assert
    Assert.True(handled);
    Assert.Equal(
        StatusCodes.Status500InternalServerError,
        context.Response.StatusCode);

    context.Response.Body.Position = 0;

    using var json = await JsonDocument.ParseAsync(
        context.Response.Body);

    var responseBody = json.RootElement.GetRawText();

    Assert.Contains("An unexpected error occurred.", responseBody);
    Assert.DoesNotContain(
        "Sensitive internal database details.",
        responseBody);
}

}
