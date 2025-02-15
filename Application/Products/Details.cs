using Application.Products.Dtos;
using ECommerce.Core.Application;
using MediatR;

namespace Application.Products
{
    public class Details
    {
        public record Query(Guid Id) : IRequest<Result<ProductDto>>;

        public class Handler : IRequestHandler<Query, Result<ProductDto>>
        {
            public Handler()
            {
            }

            public async Task<Result<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                throw new Exception("Not implemented");

                // var product = await _productRepository.GetProduct(ProductId.Of(request.Id));

                // if (product == null) return Result<ProductDto>.Failure(new ProductNotFoundException());

                // return Result<ProductDto>.Success(product.ToDto());
            }
        }
    }
}