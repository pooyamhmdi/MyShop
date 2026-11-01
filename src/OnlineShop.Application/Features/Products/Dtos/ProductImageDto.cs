namespace OnlineShop.Application.Features.Products.Dtos;

public record ProductImageDto(
    long Id,
    string ImageUrl,
    bool IsMain,
    int DisplayOrder
);