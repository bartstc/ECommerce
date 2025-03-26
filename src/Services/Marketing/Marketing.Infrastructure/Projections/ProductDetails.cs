namespace Marketing.Infrastructure.Projections;

public record ProductDetails(
    Guid Id,
    double RatingRate,
    int RatingCount,
    ProductStatus Status,
    DateTime AddedAt,
    DateTime? UpdatedAt,
    DateTime? DeletedAt);