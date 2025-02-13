namespace Application.Products.Dtos
{
    public record CreateProductDto(
        string Name,
        string Description,
        CreatePriceDto Price,
        CreateRatingDto Rating,
        string ImageUrl,
        string Category
    );

    public record CreatePriceDto(
        decimal Amount,
        string Code
    );

    public record CreateRatingDto(
        double Rate,
        int Count
    );
}