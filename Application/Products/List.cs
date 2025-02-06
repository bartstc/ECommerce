using Application.Core;
using Application.Dtos;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Products
{
    public class List
    {
        public record Query() : IRequest<Result<List<ProductDto>>>;

        public class Handler : IRequestHandler<Query, Result<List<ProductDto>>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<ProductDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var products = await _context.Products.ToListAsync();
                var productDtos = products.Select(p => p.ToDto()).ToList();

                return Result<List<ProductDto>>.Success(productDtos);
            }
        }
    }
}