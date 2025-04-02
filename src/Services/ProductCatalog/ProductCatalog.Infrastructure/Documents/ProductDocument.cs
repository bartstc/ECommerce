namespace ProductCatalog.Infrastructure.Documents;

public record ProductDocument(
    Guid ProductId,
    Category Category,
    string Name,
    string Description,
    string ImageUrl,
    decimal PriceAmount,
    string PriceCode,
    ProductStatus Status,
    DateTime AddedAt,
    DateTime? UpdatedAt,
    DateTime? DeletedAt);