using Application.Dtos;
using Application.Mappers;
using Application.Validators;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Products
{
    public class Create
    {
        public record Command(CreateProductDto ProductDto) : IRequest;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.ProductDto).SetValidator(new ProductValidator());
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var product = request.ProductDto.ToDomain();

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}