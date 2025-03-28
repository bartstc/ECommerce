using Marten.Events.Aggregation;

namespace ProductCatalog.Infrastructure.Projections;

public class ProductDetailsProjection : SingleStreamProjection<ProductDetails>
{
    public ProductDetailsProjection()
    {
        ProjectionName = nameof(ProductDetails);
        // Increase projection version by 1 whenever the serialized format of the projection has breaking changes
        // https://jeremydmiller.com/2024/03/05/marten-7-makes-write-model-projections-super/
        ProjectionVersion = 1;
    }

    public static ProductDetails Create(ProductEvent.ProductAdded @event) =>
        new(
            Id: @event.ProductId,
            Name: @event.Name,
            Category: @event.Category,
            Description: @event.Description,
            ImageUrl: @event.ImageUrl,
            PriceAmount: @event.PriceAmount,
            PriceCode: @event.PriceCode,
            Status: ProductStatus.Active,
            AddedAt: @event.Timestamp,
            UpdatedAt: null,
            DeletedAt: null);

    public static ProductDetails Apply(ProductEvent.ProductUpdated @event, ProductDetails current) =>
        current with
        {
            Name = @event.Name,
            Description = @event.Description,
            PriceAmount = @event.PriceAmount,
            PriceCode = @event.PriceCode,
            ImageUrl = @event.ImageUrl,
            Category = @event.Category,
            UpdatedAt = @event.Timestamp
        };

    public static ProductDetails Apply(ProductEvent.ProductDeleted @event, ProductDetails current) =>
        current with
        {
            Status = ProductStatus.Deleted,
            DeletedAt = @event.Timestamp
        };
}