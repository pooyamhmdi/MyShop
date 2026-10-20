using Microsoft.EntityFrameworkCore;
using OnlineShop.Infrastracture.Persistence;
var builder = WebApplication.CreateBuilder(args);

//با این کار، ASP.NET Core می‌فهمه که برای OnlineShopDbContext از SQL Server استفاده کنه.
builder.Services.AddDbContext<OnlineShopDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");
app.MapGet("/api/products", async (OnlineShopDbContext dbContext) =>
{
    var products = await dbContext.Products
        .Select(p => new
        {
            p.Id,
            p.Name,
            p.Description,
            p.BrandId
        })
        .ToListAsync();

    return Results.Ok(products);
});
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
