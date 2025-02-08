namespace Application.Stores.Dtos
{
    public record StoreDto(
        Guid Id,
        string Name,
        string Description,
        DateTime CreatedAt,
        DateTime? EditedAt
    );
}