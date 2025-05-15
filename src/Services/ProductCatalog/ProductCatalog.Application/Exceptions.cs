namespace ProductCatalog.Application;

public abstract record ProductException(string Message)
{
    public sealed record NotFound() : ProductException("Product not found");
}