namespace ProductCatalog.Infrastructure.Projections;

public record ProductDetails(
    Guid Id,
    string Name,
    Category Category,
    string Description,
    string ImageUrl,
    decimal PriceAmount,
    string PriceCode,
    double RatingRate,
    int RatingCount,
    ProductStatus Status,
    DateTime AddedAt,
    DateTime? UpdatedAt,
    DateTime? DeletedAt);