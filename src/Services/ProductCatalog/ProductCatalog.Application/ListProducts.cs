using ProductCatalog.Application.Extensions;
using ProductCatalog.Infrastructure.Documents;

namespace ProductCatalog.Application.Products;

public class ListProducts
{
    public record Query : IRequest<Result<List<ProductDocument>>>;

    public class Handler : IRequestHandler<Query, Result<List<ProductDocument>>>
    {
        private readonly IQuerySession _querySession;

        public Handler(IQuerySession querySession)
        {
            _querySession = querySession;
        }

        public async Task<Result<List<ProductDocument>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var products = await _querySession
                .QueryActiveProducts()
                .ToListAsync(cancellationToken);

            var productDtos = products.Select(p => p).ToList();

            return Result<List<ProductDocument>>.Success(productDtos);
        }
    }
}
