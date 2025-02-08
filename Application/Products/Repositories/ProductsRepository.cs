using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Products
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly DataContext _context;

        public ProductsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProduct(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<bool> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var existingProduct = await _context.Products
              .Include(p => p.Price)
              .Include(p => p.Rating)
              .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existingProduct == null)
            {
                return false;
            }

            existingProduct.Title = product.Title;
            existingProduct.Description = product.Description;
            existingProduct.Image = product.Image;
            existingProduct.Category = product.Category;
            existingProduct.EditedAt = DateTime.UtcNow;

            existingProduct.Price = new Money(product.Price.Amount, product.Price.Currency);
            existingProduct.Rating = new Rating(product.Rating.Rate, product.Rating.Count);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteProduct(Product product)
        {
            _context.Remove(product);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}