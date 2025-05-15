using ProductCatalog.Infrastructure.Documents;

namespace ProductCatalog.Application;

public class DeleteProduct
{
    public record Command(ProductId ProductId)
        : ICommand<OneOf<Unit, ProductException.NotFound, CoreException.BusinessRuleError, CoreException.Error>>;

    public class Handler(IEventStoreRepository<Product> productRepository, IQuerySession querySession)
        : ICommandHandler<Command,
            OneOf<Unit, ProductException.NotFound, CoreException.BusinessRuleError, CoreException.Error>>
    {
        public async
            Task<OneOf<Unit, ProductException.NotFound, CoreException.BusinessRuleError, CoreException.Error>> Handle(
                Command request, CancellationToken cancellationToken)
        {
            try
            {
                var stream =
                    await productRepository.FetchForWriting<Product>(request.ProductId.Value, cancellationToken);
                var document =
                    await querySession.LoadAsync<ProductDocument>(request.ProductId.Value, cancellationToken);

                if (stream.Aggregate == null) return new ProductException.NotFound();
                if (document == null) return new ProductException.NotFound();

                if (document.Status == ProductStatus.Deleted) return new ProductException.NotFound();

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
            catch (BusinessRuleException businessRuleException)
            {
                return new CoreException.BusinessRuleError(businessRuleException.Message);
            }
            catch (Exception ex)
            {
                return new CoreException.Error("Could not delete the product");
            }

            return Unit.Value;
        }
    }
}