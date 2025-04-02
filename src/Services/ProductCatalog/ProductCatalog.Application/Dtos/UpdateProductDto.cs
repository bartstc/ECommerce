namespace ProductCatalog.Application.Products.Dtos;

public record UpdateProductDto(
    string Name,
    string Description,
    string ImageUrl
);