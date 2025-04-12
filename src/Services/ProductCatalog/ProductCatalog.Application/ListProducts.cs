using ProductCatalog.Application.Extensions;
using ProductCatalog.Infrastructure.Documents;

namespace ProductCatalog.Application.Products;

public class ListProducts
{
    public record Query : IQuery<Result<List<ProductDocument>>>;

    public class Handler(IQuerySession querySession) : IQueryHandler<Query, Result<List<ProductDocument>>>
    {
        public async Task<Result<List<ProductDocument>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var products = await querySession
                .QueryActiveProducts()
                .ToListAsync(cancellationToken);

            var productDtos = products.Select(p => p).ToList();

            return Result<List<ProductDocument>>.Success(productDtos);
        }
    }
}