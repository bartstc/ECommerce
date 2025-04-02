namespace ProductCatalog.Application.Products.Dtos;

public record AddProductDto(
    string Name,
    string Description,
    AddPriceDto Price,
    string ImageUrl,
    string Category
);

public record AddPriceDto(
    decimal Amount,
    string Code
);
