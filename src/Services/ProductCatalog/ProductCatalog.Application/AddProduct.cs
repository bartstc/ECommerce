using Application.Products.Validators;
using FluentValidation;
using ProductCatalog.Application.Products.Dtos;
using ProductCatalog.Application.Products.Mappers;
using ProductCatalog.Infrastructure.Documents;

namespace ProductCatalog.Application.Products;

public class AddProduct
{
    public record Command(AddProductDto ProductDto) : IRequest<Result<Unit>>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.ProductDto).SetValidator(new AddProductValidator());
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

                _productWriteRepository.AppendEventsAsync(product);
                _productWriteRepository.StoreDocument(productDocument);
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