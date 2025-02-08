using Application.Core;
using Application.Products.Mappers;
using Application.Products.Dtos;
using MediatR;
using Persistence;

namespace Application.Products
{
    public class Details
    {
        public record Query(Guid Id) : IRequest<Result<ProductDto>>;

        public class Handler : IRequestHandler<Query, Result<ProductDto>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var product = await _context.Products.FindAsync(request.Id);

                if (product == null) return null;

                return Result<ProductDto>.Success(product.ToDto());
            }
        }
    }
}