using Application.Products.Validators;
using FluentValidation;
using ProductCatalog.Application.Products.Dtos;
using ProductCatalog.Application.Products.Exceptions;
using ProductCatalog.Infrastructure.Documents;

namespace Application.Products
{
    public class UpdateProduct
    {
        public record Command(ProductId ProductId, UpdateProductDto ProductDto) : IRequest<Result<Unit>>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.ProductDto).SetValidator(new UpdateProductValidator());
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
                    var document =
                        await _querySession.LoadAsync<ProductDocument>(request.ProductId.Value, cancellationToken);

                    if (document is null) return Result<Unit>.Failure(new ProductNotFoundException());
                    if (document.Status == ProductStatus.Deleted)
                        return Result<Unit>.Failure(new ProductNotFoundException());

                    var updatedDocument = document with
                    {
                        Name = request.ProductDto.Name,
                        Description = request.ProductDto.Description,
                        ImageUrl = request.ProductDto.ImageUrl,
                        UpdatedAt = DateTime.Now
                    };

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
}