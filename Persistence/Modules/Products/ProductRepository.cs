using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Modules.Products.Mappers;

namespace Persistence.Modules.Products.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProduct(Guid id)
        {
            var productEntity = await _context.Products.FindAsync(id);
            return productEntity?.ToDomain();
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products.Select(p => p.ToDomain()).ToListAsync();
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product.ToPersistence());
        }

        public async void UpdateProduct(Product product)
        {
            var existingProduct = await _context.Products
              .Include(p => p.Price)
              .Include(p => p.Rating)
              .FirstOrDefaultAsync(p => p.Id == product.Id.Value);

            if (existingProduct == null)
            {
                throw new Exception("Product not found");
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.Category = product.Category;
            existingProduct.EditedAt = DateTime.UtcNow;

            existingProduct.Price = Money.Of(product.Price.Amount, product.Price.Currency.Code);
            existingProduct.Rating = Rating.Of(product.Rating.Rate, product.Rating.Count);
        }

        public void DeleteProduct(Product product)
        {
            var productEntity = _context.Products.Local.FirstOrDefault(p => p.Id == product.Id.Value);
            if (productEntity != null)
            {
                _context.Entry(productEntity).State = EntityState.Detached;
            }
            _context.Remove(product.ToPersistence());
        }

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}