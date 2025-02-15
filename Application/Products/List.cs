using Application.Products.Dtos;
using Domain;
using ECommerce.Core.Application;
using MediatR;

namespace Application.Products
{
    public class List
    {
        public record Query() : IRequest<Result<List<ProductDto>>>;

        public class Handler : IRequestHandler<Query, Result<List<ProductDto>>>
        {
            public Handler()
            {
            }

            public async Task<Result<List<ProductDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                throw new Exception("Not implemented");
                // var products = await _productRepository.GetProducts();
                // var productDtos = products.Select(p => p.ToDto()).ToList();

                // return Result<List<ProductDto>>.Success(productDtos);
            }
        }
    }
}