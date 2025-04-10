using Marketing.Infrastructure.Projections;

namespace Marketing.Application;

public class GetProduct
{
    public record Query(ProductId ProductId) : IRequest<Result<ProductDetails>>;

    public class Handler : IRequestHandler<Query, Result<ProductDetails>>
    {
        private readonly IEventStoreRepository<Product> _productWriteRepository;

        public Handler(IEventStoreRepository<Product> productWriteRepository)
        {
            _productWriteRepository = productWriteRepository;
        }

        public async Task<Result<ProductDetails>> Handle(Query request, CancellationToken cancellationToken)
        {
            var product =
                await _productWriteRepository.FetchLatest<ProductDetails>(request.ProductId.Value, cancellationToken);

            if (product == null) return Result<ProductDetails>.Failure(new ProductNotFoundException());

            if (product.Status == ProductStatus.Archived)
                return Result<ProductDetails>.Failure(new ProductNotFoundException());

            return Result<ProductDetails>.Success(product);
        }
    }
}