namespace Application.Products.Dtos
{
    public record ProductDto(
        Guid Id,
        string Name,
        string Description,
        MoneyDto Price,
        RatingDto Rating,
        string ImageUrl,
        string Category,
        DateTime AddedAt,
        DateTime? EditedAt
    );

    public record MoneyDto(
        decimal Amount,
        string Currency
    );

    public record RatingDto(
        double Rate,
        int Count
    );
}