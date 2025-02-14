using Application.Products.Dtos;
using Domain;
using Domain.Errors;
using ECommerce.Core.Application;
using ECommerce.Core.Persistence;
using MediatR;

namespace Application.Products
{
    public class RateProduct
    {
        public record Command(Guid Id, RateProductDto rateProductDto) : IRequest<Result<Unit>>;

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IProductRepository _productRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(
                IProductRepository productRepository,
                IUnitOfWork unitOfWork
            )
            {
                _productRepository = productRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetProduct(request.Id);

                if (product == null) return Result<Unit>.Failure(ProductsError.ProductNotFound);

                product.RateProduct(request.rateProductDto.Rating);

                _productRepository.UpdateProduct(product);

                var result = await _unitOfWork.Complete();

                if (!result) return Result<Unit>.Failure(ProductsError.FailedToRateProduct);

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}