using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Features.Products.Queries;
using OnlineShop.Application.Features.Products;
using OnlineShop.Application.Features.Products.Queries.GetProductById;

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


    [HttpGet("{id:long}")]
    public async Task<ActionResult<ProductDto>> GetProductById(
        long id,
        CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery(id);
        var product = await _sender.Send(
            query,
            cancellationToken);

        return Ok(product);
    }

}
