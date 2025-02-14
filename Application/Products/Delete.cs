using Application.Products.Exceptions;
using Domain;
using ECommerce.Core.Application;
using MediatR;

namespace Application.Products
{
    public class Delete
    {
        public record Command(Guid Id) : IRequest<Result<Unit>>;

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetProduct(ProductId.Of(request.Id));

                if (product == null) return Result<Unit>.Failure(new ProductNotFoundException());

                _productRepository.DeleteProduct(product);

                var result = await _productRepository.Complete();

                if (!result) return Result<Unit>.Failure(new FailedToDeleteProductException());

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}