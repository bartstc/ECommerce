using Marketing.Application.Dtos;

namespace Marketing.Application;

public class RateProduct
{
    public record Command(ProductId ProductId, RateProductDto rateProductDto) : ICommand<Result<Unit>>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.rateProductDto.Rating).NotEmpty().InclusiveBetween(1, 5);
        }
    }

    public class Handler(IEventStoreRepository<Product> productRepository) : ICommandHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var stream =
                    await productRepository.FetchForWriting<Product>(request.ProductId.Value, cancellationToken);

                if (stream.Aggregate == null) return Result<Unit>.Failure(new ProductNotFoundException());

                stream.Aggregate.Rate(request.rateProductDto.Rating);

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