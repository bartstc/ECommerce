using ECommerce.Core.Exceptions;

namespace ProductCatalog.Application.Products.Exceptions;

public class ProductNotFoundException : ApplicationLogicException
{
    public ProductNotFoundException() : base("Product not found") { }
}