using Application.Core;
using MediatR;
using Domain;

namespace Application.Stores
{
    public class Delete
    {
        public record Command(Guid Id) : IRequest<Result<Unit>>;

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IStoresRepository _storesRepository;

            public Handler(IStoresRepository storesRepository)
            {
                _storesRepository = storesRepository;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var store = await _storesRepository.GetStore(request.Id);

                if (store == null) return null;

                var result = await _storesRepository.DeleteStore(store);

                if (!result) return Result<Unit>.Failure("Failed to delete the store");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}