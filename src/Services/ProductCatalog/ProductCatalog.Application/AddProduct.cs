using Application.Products.Validators;
using FluentValidation;
using ProductCatalog.Application.Products.Dtos;
using ProductCatalog.Application.Products.Mappers;

namespace ProductCatalog.Application.Products;

public class AddProduct
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

        public Handler(IEventStoreRepository<Product> productWriteRepository)
        {
            _productWriteRepository = productWriteRepository;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var product = Product.Create(request.ProductDto.ToProductData());

                await _productWriteRepository.AppendEventsAsync(product);
            }
            catch (Exception ex)
            {
                return Result<Unit>.FromException(ex);
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
