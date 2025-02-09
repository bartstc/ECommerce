namespace Domain
{
    public interface IProductRepository
    {
        Task<Product> GetProduct(Guid id);
        Task<List<Product>> GetProducts();
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        Task<bool> Complete();
    }
}