using Marketing.Application.Dtos;
using Marketing.Application.Validators;
using Marten;

namespace Marketing.Application;

public class CreateProduct
{
    public record Command(CreateProductDto ProductDto) : IRequest<Result<Unit>>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.ProductDto).SetValidator(new ProductValidator());
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
                var state = await _querySession.Events
                    .FetchStreamStateAsync(request.ProductDto.ProductId, cancellationToken);

                if (state != null)
                {
                    return Result<Unit>.Failure(new ProductAlreadyExistsException());
                }

                var productData = new ProductData(ProductId.Of(request.ProductDto.ProductId));

                var product = Product.Create(productData);

                _productWriteRepository.AppendEvents(product);
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