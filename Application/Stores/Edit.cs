using Application.Core;
using Application.Stores.Dtos;
using Application.Stores.Mappers;
using Application.Stores.Validators;
using FluentValidation;
using MediatR;
using Domain;

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
            private readonly IStoreRepository _storeRepository;

            public Handler(IStoreRepository storeRepository)
            {
                _storeRepository = storeRepository;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var store = await _storeRepository.GetStore(request.Id);

                if (store == null) return null;

                var updatedStore = request.StoreDto.ToDomain(store);

                var result = await _storeRepository.UpdateStore(updatedStore);

                if (!result) return Result<Unit>.Failure("Failed to edit the store");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}