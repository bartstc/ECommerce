using EcommerceDDD.Core.Domain;

namespace Domain.Events;

public record class ProductDeleted : DomainEvent
{
    public Guid ProductId { get; set; }

    public ProductDeleted(Guid productId)
    {
        ProductId = productId;
    }

    public static ProductDeleted Create(Guid productId)
    {
        if (productId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(productId));

        return new ProductDeleted(productId);
    }
}

