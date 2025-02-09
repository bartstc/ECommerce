using Application.Core;
using Application.Stores.Dtos;
using Application.Stores.Mappers;
using MediatR;
using Domain;
using Domain.Errors;

namespace Application.Stores
{
    public class Details
    {
        public record Query(Guid Id) : IRequest<Result<StoreDto>>;

        public class Handler : IRequestHandler<Query, Result<StoreDto>>
        {
            private readonly IStoreRepository _storeRepository;

            public Handler(IStoreRepository storeRepository)
            {
                _storeRepository = storeRepository;
            }

            public async Task<Result<StoreDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var store = await _storeRepository.GetStore(request.Id);

                if (store == null) return Result<StoreDto>.Failure(StoresError.StoreNotFound);

                return Result<StoreDto>.Success(store.ToDto());
            }
        }
    }
}