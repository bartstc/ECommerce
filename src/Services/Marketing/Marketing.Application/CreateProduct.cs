namespace Marketing.Application;

public class CreateProduct
{
    public record Command(CreateProductDto ProductDto) : ICommand<Result<Unit>>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.ProductDto).SetValidator(new ProductValidator());
        }
    }

    public class Handler(IEventStoreRepository<Product> productRepository, IQuerySession querySession)
        : ICommandHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var state = await querySession.Events
                    .FetchStreamStateAsync(request.ProductDto.ProductId, cancellationToken);

                if (state != null)
                {
                    return Result<Unit>.Failure(new ProductAlreadyExistsException());
                }

                var productData = new ProductData(ProductId.Of(request.ProductDto.ProductId));

                var product = Product.Create(productData);

                productRepository.AppendEvents(product);
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