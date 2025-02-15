using EcommerceDDD.Core.Domain;

namespace Domain.Events;

public record class ProductRated : DomainEvent
{
    public Guid ProductId { get; set; }
    public double Rating { get; set; }

    public ProductRated(Guid productId, double rating)
    {
        ProductId = productId;
        Rating = rating;
    }

    public static ProductRated Create(Guid productId, double rating)
    {
        if (productId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(productId));
        if (rating <= 0)
            throw new ArgumentOutOfRangeException(nameof(rating));

        return new ProductRated(productId, rating);
    }
}
