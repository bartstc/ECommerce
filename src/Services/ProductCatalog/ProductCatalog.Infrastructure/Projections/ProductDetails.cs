namespace ProductCatalog.Infrastructure.Projections;

public record ProductDetails(
    Guid Id,
    Category Category,
    decimal PriceAmount,
    string PriceCode,
    ProductStatus Status,
    DateTime AddedAt,
    DateTime? UpdatedAt,
    DateTime? DeletedAt);