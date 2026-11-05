using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Features.Products.Queries.GetProducts;
using OnlineShop.Application.Features.Products.Dtos;
using OnlineShop.Application.Features.Products.Queries.GetProductVariants;
using OnlineShop.Application.Features.Products.Queries.GetProductById;
using OnlineShop.Api.Controllers.Requests;
using OnlineShop.Application.Features.Products.Queries.GetProductVariantById;

namespace OnlineShop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ISender _sender;

    public ProductsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new GetProductsQuery(pageNumber, pageSize);

        var result = await _sender.Send(query, cancellationToken);

        return Ok(result);
    }
    [HttpGet("variants")]
    public async Task<IActionResult> GetProductVariants(
        [FromQuery] GetProductVariantsRequest request,
        CancellationToken cancellationToken = default)
    {
        var filter = new ProductVariantFilter(
            request.SearchTerm,
            request.BrandId,
            request.CategoryId,
            request.SortBy);
        var query = new GetProductVariantsQuery(
            request.PageNumber,
            request.PageSize,
            filter);
        var result = await _sender.Send(
            query,
            cancellationToken);

        return Ok(result);
    }
    [HttpGet("variants/{id:long}")]
    public async Task<IActionResult> GetProductVariantById(
        long id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetProductVariantByIdQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return Ok(result);
    }

}
