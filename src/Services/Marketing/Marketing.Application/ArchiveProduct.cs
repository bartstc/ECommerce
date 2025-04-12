namespace Marketing.Application;

public class ArchiveProduct
{
    public record Command(ProductId ProductId) : ICommand<Result<Unit>>;

    public class Handler(IEventStoreRepository<Product> productRepository) : ICommandHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var stream =
                    await productRepository.FetchForWriting<Product>(request.ProductId.Value, cancellationToken);

                if (stream.Aggregate == null) return Result<Unit>.Failure(new ProductNotFoundException());

                stream.Aggregate.Archive();

                productRepository.AppendEvents(stream.Aggregate);
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