using Application.Core;
using Application.Products.Dtos;
using Application.Products.Mappers;
using Application.Products.Validators;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Products
{
    public class Edit
    {
        public record Command(Guid Id, CreateProductDto ProductDto) : IRequest<Result<Unit>>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.ProductDto).SetValidator(new ProductValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _context.Products.FindAsync(request.Id);

                if (product == null) return null;

                var updatedProduct = request.ProductDto.ToDomain(product);

                _context.Entry(product).CurrentValues.SetValues(updatedProduct);
                _context.Entry(product).State = EntityState.Modified;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to edit the product");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}