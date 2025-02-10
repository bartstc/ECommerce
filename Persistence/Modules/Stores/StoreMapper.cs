using Domain;
using Persistence.Modules.Stores.Entities;
using Persistence.Modules.Products.Mappers;

namespace Persistence.Modules.Stores.Mappers
{
    public static class StoreMapper
    {
        // public static Store Create(this StoreEntity storeEntity)
        // {
        //     return new Store
        //     {
        //         Id = Guid.NewGuid(),
        //         Name = storeEntity.Name,
        //         Description = storeEntity.Description,
        //         Rating = new Rating(0, 0),
        //         Products = new List<Product>(),
        //         CreatedAt = DateTime.UtcNow
        //     };
        // }

        public static Store ToDomain(this StoreEntity storeEntity)
        {
            return new Store
            {
                Id = storeEntity.Id,
                Name = storeEntity.Name,
                Description = storeEntity.Description,
                Rating = storeEntity.Rating,
                Products = storeEntity.Products?.Select(p => p.ToDomain()).ToList(),
                CreatedAt = storeEntity.CreatedAt,
                EditedAt = storeEntity.EditedAt
            };
        }

        public static StoreEntity ToPersistence(this Store store)
        {
            return new StoreEntity
            {
                Id = store.Id,
                Name = store.Name,
                Description = store.Description,
                Rating = store.Rating,
                Products = store.Products.Select(p => p.ToPersistence()).ToList(),
                CreatedAt = store.CreatedAt,
                EditedAt = store.EditedAt
            };
        }
    }
}