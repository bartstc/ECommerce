using ECommerce.Core.Domain;

namespace Ecommerce.Core.Domain;

public record DomainEvent : IDomainEvent
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}
