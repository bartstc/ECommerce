using Marketing.Application.Products.Exceptions;

namespace Marketing.Application;

public class ArchiveProduct
{
    public record Command(ProductId ProductId) : IRequest<Result<Unit>>;

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IEventStoreRepository<Product> _productWriteRepository;

        public Handler(IEventStoreRepository<Product> productWriteRepository)
        {
            _productWriteRepository = productWriteRepository;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var stream = await _productWriteRepository.FetchForWriting<Product>(request.ProductId.Value, cancellationToken);

                if (stream.Aggregate == null) return Result<Unit>.Failure(new ProductNotFoundException());

                stream.Aggregate.Archive();

                _productWriteRepository.AppendEvents(stream.Aggregate);
                await _productWriteRepository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result<Unit>.FromException(ex);
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}