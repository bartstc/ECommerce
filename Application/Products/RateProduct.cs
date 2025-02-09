using Application.Core;
using Application.Interfaces;
using Application.Products.Dtos;
using Domain;
using MediatR;

namespace Application.Products
{
    public class RateProduct
    {
        public record Command(Guid Id, RateProductDto rateProductDto) : IRequest<Result<Unit>>;

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IProductRepository _productRepository;
            private readonly IStoreRepository _storeRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(
                IProductRepository productRepository,
                IStoreRepository storeRepository,
                IUnitOfWork unitOfWork
            )
            {
                _storeRepository = storeRepository;
                _productRepository = productRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetProduct(request.Id);

                if (product == null) return null;

                var store = await _storeRepository.GetStore(product.StoreId);

                if (store == null) return null;

                product.RateProduct(request.rateProductDto.Rating);
                store.RecalculateRating(request.rateProductDto.Rating);

                _productRepository.UpdateProduct(product);
                _storeRepository.UpdateStore(store);

                var result = await _unitOfWork.Complete();

                if (!result) return Result<Unit>.Failure("Failed to rate the product");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}