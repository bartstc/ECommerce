namespace Application.Products.Dtos
{
    public record CreateProductDto(
        string Title,
        string Description,
        CreatePriceDto Price,
        CreateRatingDto Rating,
        string Image,
        string Category
    );

    public record CreatePriceDto(
        decimal Amount,
        string Currency
    );

    public record CreateRatingDto(
        double Rate,
        int Count
    );
}