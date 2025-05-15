using ProductCatalog.Application.Products.Dtos;
using ProductCatalog.Infrastructure.Documents;

namespace ProductCatalog.Application
{
    public class UpdateProduct
    {
        public record Command(ProductId ProductId, UpdateProductDto ProductDto)
            : ICommand<OneOf<Unit, ProductException.NotFound, CoreException.BusinessRuleError, CoreException.Error>>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.ProductDto).SetValidator(new UpdateProductValidator());
            }
        }

        public class Handler(IEventStoreRepository<Product> productRepository, IQuerySession querySession)
            : ICommandHandler<Command, OneOf<Unit, ProductException.NotFound, CoreException.BusinessRuleError,
                CoreException.Error>>
        {
            public async Task<OneOf<Unit, ProductException.NotFound, CoreException.BusinessRuleError,
                CoreException.Error>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var document =
                        await querySession.LoadAsync<ProductDocument>(request.ProductId.Value, cancellationToken);

                    if (document is null) return new ProductException.NotFound();
                    if (document.Status == ProductStatus.Deleted) return new ProductException.NotFound();

                    var updatedDocument = document with
                    {
                        Name = request.ProductDto.Name,
                        Description = request.ProductDto.Description,
                        ImageUrl = request.ProductDto.ImageUrl,
                        UpdatedAt = DateTime.Now
                    };

                    productRepository.StoreDocument(updatedDocument);
                    await productRepository.SaveChangesAsync(cancellationToken);
                }
                catch (BusinessRuleException businessRuleException)
                {
                    return new CoreException.BusinessRuleError(businessRuleException.Message);
                }
                catch (Exception ex)
                {
                    return new CoreException.Error("Could not update the product");
                }

                return Unit.Value;
            }
        }
    }
}