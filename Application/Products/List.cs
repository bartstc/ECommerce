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
            private readonly IProductsRepository _productsRepository;
            public Handler(IProductsRepository productsRepository)
            {
                _productsRepository = productsRepository;
            }

            public async Task<Result<List<ProductDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var products = await _productsRepository.GetProducts();
                var productDtos = products.Select(p => p.ToDto()).ToList();

                return Result<List<ProductDto>>.Success(productDtos);
            }
        }
    }
}