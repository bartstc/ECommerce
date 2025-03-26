using Marten.Events.Aggregation;

namespace Marketing.Infrastructure.Projections;

public class ProductDetailsProjection : SingleStreamProjection<ProductDetails>
{
    public ProductDetailsProjection()
    {
        ProjectionName = nameof(ProductDetails);
        // Increase projection version by 1 whenever the serialized format of the projection has breaking changes
        // https://jeremydmiller.com/2024/03/05/marten-7-makes-write-model-projections-super/
        ProjectionVersion = 1;
    }

    public static ProductDetails Create(ProductEvent.ProductCreated @event) =>
        new(
            Id: @event.ProductId,
            RatingRate: 0,
            RatingCount: 0,
            Status: ProductStatus.Active,
            AddedAt: @event.Timestamp,
            UpdatedAt: null,
            DeletedAt: null);

    public static ProductDetails Apply(ProductEvent.ProductRated @event, ProductDetails current)
    {
        var rating = Rating.Of(current.RatingRate, current.RatingCount);
        var newRating = rating.Recalculate(@event.Rating);

        return current with
        {
            RatingRate = newRating.Rate,
            RatingCount = newRating.Count
        };
    }

    public static ProductDetails Apply(ProductEvent.ProductArchived @event, ProductDetails current) =>
        current with
        {
            Status = ProductStatus.Archived,
            DeletedAt = @event.Timestamp
        };
}