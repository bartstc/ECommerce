using Application.Products.Validators;
using FluentValidation;
using ProductCatalog.Application.Products.Dtos;
using ProductCatalog.Application.Products.Exceptions;
using ProductCatalog.Infrastructure.Documents;

namespace ProductCatalog.Application.Products;

public class UpdatePrice
{
    public record Command(ProductId ProductId, UpdatePriceDto UpdatePriceDto) : IRequest<Result<Unit>>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.UpdatePriceDto).SetValidator(new UpdatePriceValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IEventStoreRepository<Product> _productWriteRepository;
        private readonly IQuerySession _querySession;

        public Handler(IEventStoreRepository<Product> productWriteRepository, IQuerySession querySession)
        {
            _productWriteRepository = productWriteRepository;
            _querySession = querySession;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var stream =
                    await _productWriteRepository.FetchForWriting<Product>(request.ProductId.Value, cancellationToken);
                var document =
                    await _querySession.LoadAsync<ProductDocument>(request.ProductId.Value, cancellationToken);

                if (stream.Aggregate == null) return Result<Unit>.Failure(new ProductNotFoundException());
                if (document is null) return Result<Unit>.Failure(new ProductNotFoundException());

                if (document.Status == ProductStatus.Deleted)
                    return Result<Unit>.Failure(new ProductNotFoundException());

                stream.Aggregate.UpdatePrice(Money.Of(request.UpdatePriceDto.Amount, request.UpdatePriceDto.Code));

                var updatedDocument = document with
                {
                    PriceAmount = request.UpdatePriceDto.Amount,
                    PriceCode = request.UpdatePriceDto.Code,
                    UpdatedAt = DateTime.Now
                };

                _productWriteRepository.AppendEventsAsync(stream.Aggregate);
                _productWriteRepository.StoreDocument(updatedDocument);
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