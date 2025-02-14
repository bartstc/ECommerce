using Application.Products.Dtos;
using Application.Products.Exceptions;
using Application.Products.Mappers;
using Domain;
using ECommerce.Core.Application;
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
                var product = await _productRepository.GetProduct(ProductId.Of(request.Id));

                if (product == null) return Result<ProductDto>.Failure(new ProductNotFoundException());

                return Result<ProductDto>.Success(product.ToDto());
            }
        }
    }
}