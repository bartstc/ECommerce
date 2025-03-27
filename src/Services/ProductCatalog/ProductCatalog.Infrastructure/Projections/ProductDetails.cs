namespace ProductCatalog.Infrastructure.Projections;

public record ProductDetails(
    Guid Id,
    string Name,
    Category Category,
    string Description,
    string ImageUrl,
    decimal PriceAmount,
    string PriceCode,
    ProductStatus Status,
    DateTime AddedAt,
    DateTime? UpdatedAt,
    DateTime? DeletedAt);