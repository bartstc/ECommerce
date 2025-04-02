using ProductCatalog.Infrastructure.Documents;

namespace ProductCatalog.Application.Extensions;

public static class QuerySessionExtensions
{
    public static IQueryable<ProductDocument> QueryActiveProducts(this IQuerySession session)
    {
        return session
            .Query<ProductDocument>()
            .Where(p => p.Status == ProductStatus.Active);
    }
}