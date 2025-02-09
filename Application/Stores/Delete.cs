using Application.Core;
using MediatR;
using Domain;
using Domain.Errors;

namespace Application.Stores
{
    public class Delete
    {
        public record Command(Guid Id) : IRequest<Result<Unit>>;

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

                if (store == null) return Result<Unit>.Failure(StoresError.StoreNotFound);

                _storeRepository.DeleteStore(store);

                var result = await _storeRepository.Complete();

                if (!result) return Result<Unit>.Failure(StoresError.FailedToDeleteStore);

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}