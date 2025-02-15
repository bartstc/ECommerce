using ECommerce.Core.Domain;

namespace EcommerceDDD.Core.Domain;

public record class DomainEvent : IDomainEvent
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}
