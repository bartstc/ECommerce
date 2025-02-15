using Application.Products.Dtos;
using Application.Products.Mappers;
using ECommerce.Core.Application;
using Marten;
using MediatR;
using Persistence.Projections;

namespace Application.Products
{
    public class GetProducts
    {
        public record Query : IRequest<Result<List<ProductDto>>>;

        public class Handler : IRequestHandler<Query, Result<List<ProductDto>>>
        {
            private readonly IQuerySession _querySession;

            public Handler(IQuerySession querySession)
            {
                _querySession = querySession;
            }

            public async Task<Result<List<ProductDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var products = await _querySession.Query<ProductDetails>().ToListAsync(cancellationToken);

                var productDtos = products.Select(product => product.ToDto()).ToList();

                return Result<List<ProductDto>>.Success(productDtos);
            }
        }
    }
}