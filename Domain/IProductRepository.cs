namespace Domain
{
    public interface IProductRepository
    {
        Task<Product> GetProduct(Guid id);
        Task<List<Product>> GetProducts();
        Task<bool> CreateProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(Product product);
    }
}