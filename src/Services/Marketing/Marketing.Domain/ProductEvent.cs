namespace Marketing.Domain;

public abstract record ProductEvent
{
    public record ProductCreated(Guid ProductId) : DomainEvent;

    public record ProductRated(Guid ProductId, double Rating) : DomainEvent;

    public record ProductDeleted(Guid ProductId) : DomainEvent;
}

