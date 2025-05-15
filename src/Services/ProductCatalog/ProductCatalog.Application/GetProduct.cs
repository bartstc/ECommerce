using ProductCatalog.Infrastructure.Documents;

namespace ProductCatalog.Application;

public class GetProduct
{
    public record Query(ProductId ProductId) : IQuery<OneOf<ProductDocument, ProductException.NotFound>>;

    public class Handler(IQuerySession querySession)
        : IQueryHandler<Query, OneOf<ProductDocument, ProductException.NotFound>>
    {
        public async Task<OneOf<ProductDocument, ProductException.NotFound>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var document = await querySession.LoadAsync<ProductDocument>(request.ProductId.Value, cancellationToken);

            if (document == null) return new ProductException.NotFound();

            if (document.Status == ProductStatus.Deleted) return new ProductException.NotFound();

            return document;
        }
    }
}