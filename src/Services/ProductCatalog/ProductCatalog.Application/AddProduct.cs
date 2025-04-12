using ProductCatalog.Application.Products.Dtos;
using ProductCatalog.Application.Products.Mappers;
using ProductCatalog.Infrastructure.Documents;

namespace ProductCatalog.Application.Products;

public class AddProduct
{
    public record Command(AddProductDto ProductDto) : ICommand<Result<Unit>>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.ProductDto).SetValidator(new AddProductValidator());
        }
    }

    public class Handler(IEventStoreRepository<Product> productRepository) : ICommandHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
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
            catch (Exception ex)
            {
                return Result<Unit>.FromException(ex);
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}