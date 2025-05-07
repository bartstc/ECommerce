namespace Marketing.Application;

public class CreateProduct
{
    public record Command(CreateProductDto ProductDto)
        : ICommand<OneOf<Unit, CoreException.Error, CoreException.BusinessRuleError,
            ProductException.AlreadyExists>>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.ProductDto).SetValidator(new ProductValidator());
        }
    }

    public class Handler(IEventStoreRepository<Product> productRepository, IQuerySession querySession)
        : ICommandHandler<Command,
            OneOf<Unit, CoreException.Error, CoreException.BusinessRuleError, ProductException.AlreadyExists>>
    {
        public async Task<OneOf<Unit, CoreException.Error, CoreException.BusinessRuleError,
                ProductException.AlreadyExists>>
            Handle(
                Command request, CancellationToken cancellationToken)
        {
            try
            {
                var state = await querySession.Events
                    .FetchStreamStateAsync(request.ProductDto.ProductId, cancellationToken);

                if (state != null)
                {
                    return new ProductException.AlreadyExists();
                }

                var productData = new ProductData(ProductId.Of(request.ProductDto.ProductId));

                var product = Product.Create(productData);

                productRepository.AppendEvents(product);
                await productRepository.SaveChangesAsync(cancellationToken);
            }
            catch (BusinessRuleException businessRuleException)
            {
                return new CoreException.BusinessRuleError(businessRuleException.Message);
            }
            catch (Exception ex)
            {
                return new CoreException.Error("Could not create a product");
            }

            return Unit.Value;
        }
    }
}