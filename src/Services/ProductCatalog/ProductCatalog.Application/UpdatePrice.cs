using ProductCatalog.Application.Products.Dtos;
using ProductCatalog.Infrastructure.Documents;

namespace ProductCatalog.Application.Products;

public class UpdatePrice
{
    public record Command(ProductId ProductId, UpdatePriceDto UpdatePriceDto) : ICommand<Result<Unit>>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.UpdatePriceDto).SetValidator(new UpdatePriceValidator());
        }
    }

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

                if (stream.Aggregate == null) return Result<Unit>.Failure(new ProductException());
                if (document is null) return Result<Unit>.Failure(new ProductException());

                if (document.Status == ProductStatus.Deleted)
                    return Result<Unit>.Failure(new ProductException());

                stream.Aggregate.UpdatePrice(Money.Of(request.UpdatePriceDto.Amount, request.UpdatePriceDto.Code));

                var updatedDocument = document with
                {
                    PriceAmount = request.UpdatePriceDto.Amount,
                    PriceCode = request.UpdatePriceDto.Code,
                    UpdatedAt = DateTime.Now
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