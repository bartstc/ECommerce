using Application.Core;
using Application.Stores.Mappers;
using Application.Stores.Dtos;
using Application.Stores.Validators;
using FluentValidation;
using MediatR;
using Domain;

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
            private readonly IStoreRepository _storeRepository;

            public Handler(IStoreRepository storeRepository)
            {
                _storeRepository = storeRepository;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var store = request.StoreDto.ToDomain();

                _storeRepository.CreateStore(store);

                var result = await _storeRepository.Complete();

                if (!result) return Result<Unit>.Failure("Failed to create store");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}