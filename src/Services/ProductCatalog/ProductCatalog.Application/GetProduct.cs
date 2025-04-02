using ProductCatalog.Application.Products.Exceptions;
using ProductCatalog.Infrastructure.Documents;

namespace ProductCatalog.Application.Products;

public class GetProduct
{
    public record Query(ProductId ProductId) : IRequest<Result<ProductDocument>>;

    public class Handler : IRequestHandler<Query, Result<ProductDocument>>
    {
        private readonly IQuerySession _querySession;

        public Handler(IQuerySession querySession)

        {
            _querySession = querySession;
        }

        public async Task<Result<ProductDocument>> Handle(Query request, CancellationToken cancellationToken)
        {
            var document = await _querySession.LoadAsync<ProductDocument>(request.ProductId.Value);

            if (document == null) return Result<ProductDocument>.Failure(new ProductNotFoundException());

            if (document.Status == ProductStatus.Deleted)
                return Result<ProductDocument>.Failure(new ProductNotFoundException());

            return Result<ProductDocument>.Success(document);
        }
    }
}