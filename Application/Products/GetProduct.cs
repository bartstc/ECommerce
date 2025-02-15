using Application.Products.Dtos;
using Application.Products.Exceptions;
using Application.Products.Mappers;
using Domain;
using ECommerce.Core.Application;
using Marten;
using MediatR;
using Persistence.Projections;

namespace Application.Products
{
    public class GetProduct
    {
        public record Query(ProductId ProductId) : IRequest<Result<ProductDto>>;

        public class Handler : IRequestHandler<Query, Result<ProductDto>>
        {
            private readonly IQuerySession _querySession;
            public Handler(IQuerySession querySession)

            {
                _querySession = querySession;
            }

            public async Task<Result<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var product = await _querySession.LoadAsync<ProductDetails>(request.ProductId.Value);

                if (product == null) return Result<ProductDto>.Failure(new ProductNotFoundException());

                return Result<ProductDto>.Success(product.ToDto());
            }
        }
    }
}