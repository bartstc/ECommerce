using Application.Products.Exceptions;
using Domain;
using ECommerce.Core.Application;
using ECommerce.Core.Persistence;
using MediatR;

namespace Application.Products
{
    public class DeleteProduct
    {
        public record Command(ProductId ProductId) : IRequest<Result<Unit>>;

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
                    var stream = await _productWriteRepository.FetchForWriting<Product>(request.ProductId.Value);

                    if (stream.Aggregate == null) return Result<Unit>.Failure(new ProductNotFoundException());

                    stream.Aggregate.Delete();

                    await _productWriteRepository.AppendEventsAsync(stream.Aggregate);
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