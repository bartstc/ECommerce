using Marten;
using Marten.Events.Projections;

namespace Persistence.Projections;

public static class ProjectionsConfiguration
{
    public static void ConfigureProjections(this StoreOptions options)
    {
        options.Projections.Add<ProductDetailsProjection>(ProjectionLifecycle.Async);
    }
}
