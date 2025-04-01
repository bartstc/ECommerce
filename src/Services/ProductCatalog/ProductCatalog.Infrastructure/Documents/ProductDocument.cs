namespace ProductCatalog.Infrastructure.Documents;

public record ProductDocument(
    Guid ProductId,
    string Name,
    string Description,
    string ImageUrl);