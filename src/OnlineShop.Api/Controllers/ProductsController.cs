using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Infrastracture.Persistence;

namespace OnlineShop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly OnlineShopDbContext _dbContext;

    public ProductsController(OnlineShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _dbContext.Products
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Description,
                p.BrandId,

                Variants = p.Variants.Select(v => new
                {
                    v.Id,
                    v.SKU,
                    v.Price,
                    v.Stock
                })
            })
            .ToListAsync();

        return Ok(products);
    }
}
