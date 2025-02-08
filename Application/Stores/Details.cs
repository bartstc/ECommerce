using Application.Core;
using Application.Stores.Dtos;
using Application.Stores.Mappers;
using MediatR;
using Domain;

namespace Application.Stores
{
    public class Details
    {
        public record Query(Guid Id) : IRequest<Result<StoreDto>>;

        public class Handler : IRequestHandler<Query, Result<StoreDto>>
        {
            private readonly IStoresRepository _storesRepository;

            public Handler(IStoresRepository storesRepository)
            {
                _storesRepository = storesRepository;
            }

            public async Task<Result<StoreDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var store = await _storesRepository.GetStore(request.Id);

                if (store == null) return null;

                return Result<StoreDto>.Success(store.ToDto());
            }
        }
    }
}