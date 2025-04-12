using ProductCatalog.Application.Products.Exceptions;
using ProductCatalog.Infrastructure.Documents;

namespace ProductCatalog.Application.Products;

public class GetProduct
{
    public record Query(ProductId ProductId) : IQuery<Result<ProductDocument>>;

    public class Handler(IQuerySession querySession) : IQueryHandler<Query, Result<ProductDocument>>
    {
        public async Task<Result<ProductDocument>> Handle(Query request, CancellationToken cancellationToken)
        {
            var document = await querySession.LoadAsync<ProductDocument>(request.ProductId.Value, cancellationToken);

            if (document == null) return Result<ProductDocument>.Failure(new ProductNotFoundException());

            if (document.Status == ProductStatus.Deleted)
                return Result<ProductDocument>.Failure(new ProductNotFoundException());

            return Result<ProductDocument>.Success(document);
        }
    }
}