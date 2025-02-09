using Application.Core;
using Domain;
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
                var product = await _productRepository.GetProduct(request.Id);

                if (product == null) return null;

                var result = await _productRepository.DeleteProduct(product);

                if (!result) return Result<Unit>.Failure("Failed to delete the product");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}