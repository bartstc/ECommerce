using Domain.Events;
using Marten.Events.Aggregation;

namespace Persistence.Projections;

public class ProductDetailsProjection : SingleStreamProjection<ProductDetails>
{
    public ProductDetailsProjection()
    {
        ProjectEvent<ProductAdded>((item, @event) => item.Apply(@event));
        ProjectEvent<ProductUpdated>((item, @event) => item.Apply(@event));
        ProjectEvent<ProductRated>((item, @event) => item.Apply(@event));
        ProjectEvent<ProductDeleted>((item, @event) => item.Apply(@event));
    }
}
