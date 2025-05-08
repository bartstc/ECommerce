using Marketing.Infrastructure.Projections;

namespace Marketing.Application;

public class GetProduct
{
    public record Query(ProductId ProductId) : IQuery<OneOf<ProductDetails, ProductException.NotFound>>;

    public class Handler(IEventStoreRepository<Product> productRepository)
        : IQueryHandler<Query, OneOf<ProductDetails, ProductException.NotFound>>
    {
        public async Task<OneOf<ProductDetails, ProductException.NotFound>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var product =
                await productRepository.FetchLatest<ProductDetails>(request.ProductId.Value, cancellationToken);

            if (product == null) return new ProductException.NotFound();

            if (product.Status == ProductStatus.Archived) return new ProductException.NotFound();

            return product;
        }
    }
}