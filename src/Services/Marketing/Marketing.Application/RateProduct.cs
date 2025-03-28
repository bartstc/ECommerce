using Marketing.Application.Dtos;
using Marketing.Application.Products.Exceptions;

namespace Marketing.Application;

public class RateProduct
{
    public record Command(ProductId ProductId, RateProductDto rateProductDto) : IRequest<Result<Unit>>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.rateProductDto.Rating).NotEmpty().InclusiveBetween(1, 5);
        }
    }

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
                var stream = await _productWriteRepository.FetchForWriting<Product>(request.ProductId.Value);

                if (stream.Aggregate == null) return Result<Unit>.Failure(new ProductNotFoundException());

                stream.Aggregate.Rate(request.rateProductDto.Rating);

                await _productWriteRepository.AppendEventsAsync(stream.Aggregate);
            }
            catch (Exception ex)
            {
                return Result<Unit>.FromException(ex);
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
