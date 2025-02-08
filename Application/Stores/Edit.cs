using Application.Core;
using Application.Stores.Dtos;
using Application.Validators;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Stores
{
    public class Edit
    {
        public record Command(Guid Id, CreateStoreDto StoreDto) : IRequest<Result<Unit>>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.StoreDto).SetValidator(new StoreValidator());
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
                var store = await _context.Stores.FindAsync(request.Id);

                if (store == null) return null;

                var updatedStore = request.StoreDto.ToDomain(store);

                _context.Entry(store).CurrentValues.SetValues(updatedStore);
                _context.Entry(store).State = EntityState.Modified;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to edit the store");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}