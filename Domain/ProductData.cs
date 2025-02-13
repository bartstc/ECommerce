namespace Domain;

public record class ProductData(
    Guid? Id,
    string Name,
    string Description,
    Money Price,
    Rating Rating,
    string ImageUrl,
    Category Category,
    DateTime AddedAt,
    DateTime? EditedAt
);