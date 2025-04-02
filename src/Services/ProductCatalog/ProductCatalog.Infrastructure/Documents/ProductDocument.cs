namespace ProductCatalog.Infrastructure.Documents;

public record ProductDocument(
    Guid ProductId,
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