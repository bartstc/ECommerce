using ECommerce.Core.Exceptions;

namespace Marketing.Application.Products.Exceptions;

public class ProductNotFoundException : ApplicationLogicException
{
    public ProductNotFoundException() : base("Product not found") { }
}