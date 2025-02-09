using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Products
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
            return await _context.Products.FindAsync(id);
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }

        public async void UpdateProduct(Product product)
        {
            var existingProduct = await _context.Products
              .Include(p => p.Price)
              .Include(p => p.Rating)
              .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existingProduct == null)
            {
                throw new Exception("Product not found");
            }

            existingProduct.Title = product.Title;
            existingProduct.Description = product.Description;
            existingProduct.Image = product.Image;
            existingProduct.Category = product.Category;
            existingProduct.EditedAt = DateTime.UtcNow;

            existingProduct.Price = new Money(product.Price.Amount, product.Price.Currency);
            existingProduct.Rating = new Rating(product.Rating.Rate, product.Rating.Count);
        }

        public void DeleteProduct(Product product)
        {
            _context.Remove(product);
        }

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}