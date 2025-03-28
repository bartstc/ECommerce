namespace ProductCatalog.Domain;

public interface IProductRepository
{
    Task<Product> GetProduct(ProductId id);
    Task<List<Product>> ListProducts();
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
    Task<bool> Complete();
}
