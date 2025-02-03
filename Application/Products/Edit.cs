using Application.Dtos;
using AutoMapper;
using MediatR;
using Persistence;

namespace Application.Products
{
    public class Edit
    {
        public record Command(Guid Id, CreateProductDto ProductDto) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _context.Products.FindAsync(request.Id);

                _mapper.Map(request.ProductDto, product);
                product.EditedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
        }
    }
}