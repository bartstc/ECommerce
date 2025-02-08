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
            private readonly IProductsRepository _productsRepository;

            public Handler(IProductsRepository productsRepository)
            {
                _productsRepository = productsRepository;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _productsRepository.GetProduct(request.Id);

                if (product == null) return null;

                var result = await _productsRepository.DeleteProduct(product);

                if (!result) return Result<Unit>.Failure("Failed to delete the product");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}