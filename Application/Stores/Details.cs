using Application.Core;
using Application.Stores.Dtos;
using Application.Stores.Mappers;
using MediatR;
using Persistence;

namespace Application.Stores
{
    public class Details
    {
        public record Query(Guid Id) : IRequest<Result<StoreDto>>;

        public class Handler : IRequestHandler<Query, Result<StoreDto>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<StoreDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var store = await _context.Stores.FindAsync(request.Id);

                if (store == null) return null;

                return Result<StoreDto>.Success(store.ToDto());
            }
        }
    }
}