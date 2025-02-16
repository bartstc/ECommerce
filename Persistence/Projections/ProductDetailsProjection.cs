using Domain;
using Marten.Events.Aggregation;

namespace Persistence.Projections;

public class ProductDetailsProjection : SingleStreamProjection<ProductDetails>
{
    public ProductDetailsProjection()
    {
        ProjectEvent<ProductEvent.ProductAdded>((item, @event) => item.Apply(@event));
        ProjectEvent<ProductEvent.ProductUpdated>((item, @event) => item.Apply(@event));
        ProjectEvent<ProductEvent.ProductRated>((item, @event) => item.Apply(@event));
        ProjectEvent<ProductEvent.ProductDeleted>((item, @event) => item.Apply(@event));
    }
}
