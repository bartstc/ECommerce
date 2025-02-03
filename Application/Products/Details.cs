using Application.Dtos;
using Application.Mappers;
using MediatR;
using Persistence;

namespace Application.Products
{
    public class Details
    {
        public record Query(Guid Id) : IRequest<ProductDto>;

        public class Handler : IRequestHandler<Query, ProductDto>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<ProductDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var product = await _context.Products.FindAsync(request.Id);
                return product.ToDto();
            }
        }
    }
}