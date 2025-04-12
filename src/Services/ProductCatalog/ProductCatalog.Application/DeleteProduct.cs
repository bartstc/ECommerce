using ProductCatalog.Application.Products.Exceptions;
using ProductCatalog.Infrastructure.Documents;

namespace ProductCatalog.Application.Products;

public class DeleteProduct
{
    public record Command(ProductId ProductId) : ICommand<Result<Unit>>;

    public class Handler(IEventStoreRepository<Product> productRepository, IQuerySession querySession)
        : ICommandHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var stream =
                    await productRepository.FetchForWriting<Product>(request.ProductId.Value, cancellationToken);
                var document =
                    await querySession.LoadAsync<ProductDocument>(request.ProductId.Value, cancellationToken);

                if (stream.Aggregate == null) return Result<Unit>.Failure(new ProductNotFoundException());
                if (document == null) return Result<Unit>.Failure(new ProductNotFoundException());

                if (document.Status == ProductStatus.Deleted)
                    return Result<Unit>.Failure(new ProductNotFoundException());

                stream.Aggregate.Delete();

                var updatedDocument = document with
                {
                    Status = ProductStatus.Deleted,
                    UpdatedAt = DateTime.Now,
                    DeletedAt = DateTime.Now
                };

                productRepository.AppendEvents(stream.Aggregate);
                productRepository.StoreDocument(updatedDocument);
                await productRepository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result<Unit>.FromException(ex);
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}