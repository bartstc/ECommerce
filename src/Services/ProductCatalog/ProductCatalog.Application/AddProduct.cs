using ECommerce.Core.Exceptions;
using ProductCatalog.Application.Products.Dtos;
using ProductCatalog.Application.Products.Mappers;
using ProductCatalog.Infrastructure.Documents;

namespace ProductCatalog.Application;

public class AddProduct
{
    public record Command(AddProductDto ProductDto)
        : ICommand<OneOf<Unit, CoreException.BusinessRuleError, CoreException.Error>>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.ProductDto).SetValidator(new AddProductValidator());
        }
    }

    public class Handler(IEventStoreRepository<Product> productRepository)
        : ICommandHandler<Command, OneOf<Unit, CoreException.BusinessRuleError, CoreException.Error>>
    {
        public async Task<OneOf<Unit, CoreException.BusinessRuleError, CoreException.Error>> Handle(Command request,
            CancellationToken cancellationToken)
        {
            try
            {
                var product = Product.Create(request.ProductDto.ToProductData());

                var productDocument = new ProductDocument(
                    product.Id.Value,
                    product.Category,
                    request.ProductDto.Name,
                    request.ProductDto.Description,
                    request.ProductDto.ImageUrl,
                    request.ProductDto.Price.Amount,
                    request.ProductDto.Price.Code,
                    product.Status,
                    product.AddedAt,
                    product.UpdatedAt,
                    product.DeletedAt);

                productRepository.AppendEvents(product);
                productRepository.StoreDocument(productDocument);
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