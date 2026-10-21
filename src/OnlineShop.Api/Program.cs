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

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.MapControllers();

app.Run();

