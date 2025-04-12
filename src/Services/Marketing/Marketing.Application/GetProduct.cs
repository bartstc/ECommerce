using Marketing.Infrastructure.Projections;

namespace Marketing.Application;

public class GetProduct
{
    public record Query(ProductId ProductId) : IQuery<Result<ProductDetails>>;

    public class Handler(IEventStoreRepository<Product> productRepository)
        : IQueryHandler<Query, Result<ProductDetails>>
    {
        public async Task<Result<ProductDetails>> Handle(Query request, CancellationToken cancellationToken)
        {
            var product =
                await productRepository.FetchLatest<ProductDetails>(request.ProductId.Value, cancellationToken);

            if (product == null) return Result<ProductDetails>.Failure(new ProductNotFoundException());

            if (product.Status == ProductStatus.Archived)
                return Result<ProductDetails>.Failure(new ProductNotFoundException());

            return Result<ProductDetails>.Success(product);
        }
    }
}