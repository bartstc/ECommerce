namespace ProductCatalog.Domain;

public abstract record ProductEvent
{
    public record ProductAdded(
        Guid ProductId,
        decimal PriceAmount,
        string PriceCode,
        Category Category
    ) : DomainEvent;

    public record PriceUpdated(
        Guid ProductId,
        decimal PriceAmount,
        string PriceCode
    ) : DomainEvent;

    public record ProductDeleted(
        Guid ProductId
    ) : DomainEvent;
}