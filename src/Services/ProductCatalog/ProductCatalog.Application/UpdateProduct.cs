using Application.Products.Validators;
using FluentValidation;
using ProductCatalog.Application.Products.Dtos;
using ProductCatalog.Application.Products.Exceptions;
using ProductCatalog.Infrastructure.Documents;

namespace Application.Products
{
    public class UpdateProduct
    {
        public record Command(ProductId ProductId, UpdateProductDto ProductDto) : ICommand<Result<Unit>>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.ProductDto).SetValidator(new UpdateProductValidator());
            }
        }

        public class Handler(IEventStoreRepository<Product> productRepository, IQuerySession querySession)
            : ICommandHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var document =
                        await querySession.LoadAsync<ProductDocument>(request.ProductId.Value, cancellationToken);

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

                    productRepository.StoreDocument(updatedDocument);
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
}