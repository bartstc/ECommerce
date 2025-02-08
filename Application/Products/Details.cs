using Application.Core;
using Application.Products.Dtos;
using Application.Products.Mappers;
using Domain;
using MediatR;

namespace Application.Products
{
    public class Details
    {
        public record Query(Guid Id) : IRequest<Result<ProductDto>>;

        public class Handler : IRequestHandler<Query, Result<ProductDto>>
        {
            private readonly IProductsRepository _productsRepository;

            public Handler(IProductsRepository productsRepository)
            {
                _productsRepository = productsRepository;
            }

            public async Task<Result<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var product = await _productsRepository.GetProduct(request.Id);

                if (product == null) return null;

                return Result<ProductDto>.Success(product.ToDto());
            }
        }
    }
}