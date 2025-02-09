namespace Application.Stores.Dtos
{
    public record StoreDto(
        Guid Id,
        string Name,
        string Description,
        RatingDto Rating,
        DateTime CreatedAt,
        DateTime? EditedAt
    );

    public record RatingDto(
         double Rate,
         int Count
     );
}