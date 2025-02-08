using Application.Stores.Dtos;
using Domain;

namespace Application.Stores.Mappers
{
    public static class StoreMapper
    {
        public static StoreDto ToDto(this Store store)
        {
            return new StoreDto(
                Id: store.Id,
                Name: store.Name,
                Description: store.Description,
                CreatedAt: store.CreatedAt,
                EditedAt: store.EditedAt
            );
        }

        public static Store ToDomain(this CreateStoreDto storeDto)
        {
            return new Store
            {
                Id = Guid.NewGuid(),
                Name = storeDto.Name,
                Description = storeDto.Description,
                Products = new List<Product>(),
                CreatedAt = DateTime.UtcNow
            };
        }

        public static Store ToDomain(this CreateStoreDto storeDto, Store store)
        {
            return new Store
            {
                Id = store.Id,
                Name = storeDto.Name,
                Description = storeDto.Description,
                Products = store.Products,
                CreatedAt = store.CreatedAt,
                EditedAt = store.EditedAt
            };
        }
    }
}