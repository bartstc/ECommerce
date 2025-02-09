using Application.Core;
using Application.Products.Dtos;
using Application.Products.Mappers;
using Domain;
using Domain.Errors;
using MediatR;

namespace Application.Products
{
    public class Details
    {
        public record Query(Guid Id) : IRequest<Result<ProductDto>>;

        public class Handler : IRequestHandler<Query, Result<ProductDto>>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<Result<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetProduct(request.Id);

                if (product == null) return Result<ProductDto>.Failure(ProductsError.ProductNotFound);

                return Result<ProductDto>.Success(product.ToDto());
            }
        }
    }
}