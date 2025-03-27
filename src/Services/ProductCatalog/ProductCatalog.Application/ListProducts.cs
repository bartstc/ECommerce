using ProductCatalog.Infrastructure.Extensions;
using ProductCatalog.Infrastructure.Projections;

namespace ProductCatalog.Application.Products;

public class ListProducts
{
    public record Query : IRequest<Result<List<ProductDetails>>>;

    public class Handler : IRequestHandler<Query, Result<List<ProductDetails>>>
    {
        private readonly IQuerySession _querySession;

        public Handler(IQuerySession querySession)
        {
            _querySession = querySession;
        }

        public async Task<Result<List<ProductDetails>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var products = await _querySession
                .QueryActiveProducts()
                .ToListAsync(cancellationToken);

            var productDtos = products.Select(p => p).ToList();

            return Result<List<ProductDetails>>.Success(productDtos);
        }
    }
}
