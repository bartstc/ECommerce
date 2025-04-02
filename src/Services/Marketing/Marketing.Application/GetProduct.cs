using Marketing.Application.Products.Exceptions;
using Marketing.Infrastructure.Projections;
using Marten;

namespace Marketing.Application;

public class GetProduct
{
    public record Query(ProductId ProductId) : IRequest<Result<ProductDetails>>;

    public class Handler : IRequestHandler<Query, Result<ProductDetails>>
    {
        private readonly IQuerySession _querySession;
        public Handler(IQuerySession querySession)
        {
            _querySession = querySession;
        }

        public async Task<Result<ProductDetails>> Handle(Query request, CancellationToken cancellationToken)
        {
            var product = await _querySession.LoadAsync<ProductDetails>(request.ProductId.Value, cancellationToken);

            if (product == null) return Result<ProductDetails>.Failure(new ProductNotFoundException());

            if (product.Status == ProductStatus.Archived) return Result<ProductDetails>.Failure(new ProductNotFoundException());

            return Result<ProductDetails>.Success(product);
        }
    }
}
