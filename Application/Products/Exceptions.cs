
using ECommerce.Core.Exceptions;

namespace Application.Products.Exceptions;

public class ProductNotFoundException : ApplicationLogicException
{
    public ProductNotFoundException() : base("Product not found") { }
}

public class FailedToRateProductException : ApplicationLogicException
{
    public FailedToRateProductException() : base("Failed to rate the product") { }
}

public class FailedToCreateProductException : ApplicationLogicException
{
    public FailedToCreateProductException() : base("Failed to create the product") { }
}

public class FailedToUpdateProductException : ApplicationLogicException
{
    public FailedToUpdateProductException() : base("Failed to update the product") { }
}

public class FailedToDeleteProductException : ApplicationLogicException
{
    public FailedToDeleteProductException() : base("Failed to delete the product") { }
}
