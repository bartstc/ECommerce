namespace Application.Products.Dtos
{
    public record CreateProductDto(
        string Name,
        string Description,
        CreatePriceDto Price,
        string ImageUrl,
        string Category
    );

    public record CreatePriceDto(
        decimal Amount,
        string Code
    );
}