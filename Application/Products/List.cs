using Application.Dtos;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Products
{
    public class List
    {
        public record Query() : IRequest<List<ProductDto>>;

        public class Handler : IRequestHandler<Query, List<ProductDto>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var products = await _context.Products.ToListAsync();
                return products.Select(p => p.ToDto()).ToList();
            }
        }
    }
}