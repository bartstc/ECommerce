using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Products.Repositories
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
            return await _context.Stores
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Store> GetStoreWithProducts(Guid id)
        {
            return await _context.Stores
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public void CreateStore(Store store)
        {
            _context.Stores.Add(store);
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
            _context.Stores.Remove(store);
        }

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}