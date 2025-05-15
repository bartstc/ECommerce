using ProductCatalog.Application.Extensions;
using ProductCatalog.Infrastructure.Documents;

namespace ProductCatalog.Application;

public class ListProducts
{
    public record Query : IQuery<OneOf<List<ProductDocument>>>;

    public class Handler(IQuerySession querySession) : IQueryHandler<Query, OneOf<List<ProductDocument>>>
    {
        public async Task<OneOf<List<ProductDocument>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var products = await querySession
                .QueryActiveProducts()
                .ToListAsync(cancellationToken);

            var productDtos = products.Select(p => p).ToList();

            return productDtos;
        }
    }
}