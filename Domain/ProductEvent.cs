using EcommerceDDD.Core.Domain;

namespace Domain;

public abstract record ProductEvent
{
    public record ProductAdded(
        Guid ProductId,
        string Name,
        string Description,
        decimal PriceAmount,
        string PriceCode,
        string ImageUrl,
        Category Category
    ) : DomainEvent;

    public record ProductUpdated(
        Guid ProductId,
        string Name,
        string Description,
        decimal PriceAmount,
        string PriceCode,
        string ImageUrl,
        Category Category
    ) : DomainEvent;

    public record ProductRated(
        Guid ProductId,
        double Rating
    ) : DomainEvent;

    public record ProductDeleted(
        Guid ProductId
    ) : DomainEvent;
}
