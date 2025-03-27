using ProductCatalog.Infrastructure.Projections;

namespace ProductCatalog.Infrastructure.Extensions;

public static class QuerySessionExtensions
{
    public static IQueryable<ProductDetails> QueryActiveProducts(this IQuerySession session)
    {
        return session
            .Query<ProductDetails>()
            .Where(p => p.Status == ProductStatus.Active);
    }
}