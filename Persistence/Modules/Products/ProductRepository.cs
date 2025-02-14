using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Modules.Products.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProduct(ProductId id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            return product;
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

            var newData = new ProductData(
                product.Id.Value,
                product.Name,
                product.Description,
                product.Price,
                product.Rating,
                product.ImageUrl,
                product.Category,
                product.AddedAt,
                DateTime.UtcNow
            );

            existingProduct.Update(newData);

            _context.Products.Update(existingProduct);
        }

        public void DeleteProduct(Product product)
        {
            var productEntity = _context.Products.Local.FirstOrDefault(p => p.Id == product.Id);
            if (productEntity != null)
            {
                _context.Entry(productEntity).State = EntityState.Detached;
            }
            _context.Remove(product);
        }

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}