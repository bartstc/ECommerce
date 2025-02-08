using Application.Core;
using Application.Mappers;
using Application.Stores.Dtos;
using Application.Validators;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Stores
{
    public class Create
    {
        public record Command(CreateStoreDto StoreDto) : IRequest<Result<Unit>>;

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
                var store = request.StoreDto.ToDomain();

                _context.Stores.Add(store);
                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create store");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}