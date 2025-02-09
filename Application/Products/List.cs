using Application.Core;
using Application.Products.Dtos;
using Application.Products.Mappers;
using Domain;
using MediatR;

namespace Application.Products
{
    public class List
    {
        public record Query() : IRequest<Result<List<ProductDto>>>;

        public class Handler : IRequestHandler<Query, Result<List<ProductDto>>>
        {
            private readonly IProductRepository _productRepository;
            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<Result<List<ProductDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var products = await _productRepository.GetProducts();
                var productDtos = products.Select(p => p.ToDto()).ToList();

                return Result<List<ProductDto>>.Success(productDtos);
            }
        }
    }
}