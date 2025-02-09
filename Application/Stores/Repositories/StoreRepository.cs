using Domain;
using Persistence;

namespace Application.Stores.Repositories
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
            return await _context.Stores.FindAsync(id);
        }

        public async Task<bool> CreateStore(Store store)
        {
            _context.Stores.Add(store);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStore(Store store)
        {
            var existingStore = await _context.Stores.FindAsync(store.Id);
            if (existingStore == null)
            {
                return false;
            }

            existingStore.Name = store.Name;
            existingStore.Description = store.Description;
            existingStore.EditedAt = DateTime.UtcNow;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteStore(Store store)
        {
            _context.Stores.Remove(store);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}