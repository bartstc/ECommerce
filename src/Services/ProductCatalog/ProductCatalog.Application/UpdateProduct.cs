using Application.Products.Validators;
using FluentValidation;
using ProductCatalog.Application.Products.Dtos;
using ProductCatalog.Application.Products.Exceptions;
using ProductCatalog.Application.Products.Mappers;
using ProductCatalog.Infrastructure.Documents;

namespace Application.Products
{
    public class UpdateProduct
    {
        public record Command(ProductId ProductId, CreateProductDto ProductDto) : IRequest<Result<Unit>>;

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
                    var stream = await _productWriteRepository.FetchForWriting<Product>(request.ProductId.Value);
                    var document = await _querySession.LoadAsync<ProductDocument>(request.ProductId.Value);

                    if (stream.Aggregate is null) return Result<Unit>.Failure(new ProductNotFoundException());
                    if (document is null) return Result<Unit>.Failure(new ProductNotFoundException());

                    stream.Aggregate.Update(request.ProductDto.ToProductData());

                    var updatedDocument = new ProductDocument(
                        request.ProductId.Value,
                        request.ProductDto.Name,
                        request.ProductDto.Description,
                        request.ProductDto.ImageUrl);

                    _productWriteRepository.AppendEventsAsync(stream.Aggregate);
                    _productWriteRepository.StoreDocument(updatedDocument);
                    await _productWriteRepository.SaveChangesAsync();
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