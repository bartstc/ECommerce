using Marten;
using Marten.Events.Projections;

namespace ProductCatalog.Infrastructure.Projections;

public static class ProjectionsConfiguration
{
    public static void ConfigureProjections(this StoreOptions options)
    {
        options.Projections.Add<ProductDetailsProjection>(ProjectionLifecycle.Async);
    }
}
