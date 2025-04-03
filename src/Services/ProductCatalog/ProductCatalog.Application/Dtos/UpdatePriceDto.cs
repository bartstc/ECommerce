namespace ProductCatalog.Application.Products.Dtos;

public record UpdatePriceDto(
    decimal Amount,
    string Code
);