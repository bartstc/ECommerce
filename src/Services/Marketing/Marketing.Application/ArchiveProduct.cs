namespace Marketing.Application;

public class ArchiveProduct
{
    public record Command(ProductId ProductId)
        : ICommand<OneOf<Unit, CoreException.Error, CoreException.BusinessRuleError,
            ProductException.NotFound>>;

    public class Handler(IEventStoreRepository<Product> productRepository) : ICommandHandler<Command,
        OneOf<Unit, CoreException.Error, CoreException.BusinessRuleError, ProductException.NotFound>>
    {
        public async
            Task<OneOf<Unit, CoreException.Error, CoreException.BusinessRuleError, ProductException.NotFound>>
            Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var stream =
                    await productRepository.FetchForWriting<Product>(request.ProductId.Value, cancellationToken);

                if (stream.Aggregate == null) return new ProductException.NotFound();

                stream.Aggregate.Archive();

                productRepository.AppendEvents(stream.Aggregate);
                await productRepository.SaveChangesAsync(cancellationToken);
            }
            catch (BusinessRuleException businessRuleException)
            {
                return new CoreException.BusinessRuleError(businessRuleException.Message);
            }
            catch (Exception ex)
            {
                return new CoreException.Error("Could not archive a product");
            }

            return Unit.Value;
        }
    }
}