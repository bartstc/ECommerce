using ECommerce.Core.Exceptions;

namespace Marketing.Application;

public class ProductNotFoundException : ApplicationLogicException
{
    public ProductNotFoundException() : base("Product not found") { }
}

public class ProductAlreadyExistsException : ApplicationLogicException
{
    public ProductAlreadyExistsException() : base("Product already exists")
    {
    }
}
