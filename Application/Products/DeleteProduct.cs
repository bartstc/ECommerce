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
                var product = await _productWriteRepository.FetchStreamAsync(request.ProductId.Value);

                if (product == null) return Result<Unit>.Failure(new ProductNotFoundException());

                product.Delete();

                await _productWriteRepository.AppendEventsAsync(product);

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}