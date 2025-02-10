using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Modules.Stores.Mappers;

namespace Persistence.Modules.Products.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly DataContext _context;

        public StoreRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Store> GetStore(Guid id)
        {
            var storeEntity = await _context.Stores
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.Id == id);

            return storeEntity.ToDomain();
        }

        public void CreateStore(Store store)
        {
            _context.Stores.Add(store.ToPersistence());
        }

        public async void UpdateStore(Store store)
        {
            var existingStore = await _context.Stores.FindAsync(store.Id);
            if (existingStore == null)
            {
                throw new Exception("Store not found");
            }

            existingStore.Name = store.Name;
            existingStore.Description = store.Description;
            existingStore.EditedAt = DateTime.UtcNow;
        }

        public void DeleteStore(Store store)
        {
            _context.Stores.Remove(store.ToPersistence());
        }

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}