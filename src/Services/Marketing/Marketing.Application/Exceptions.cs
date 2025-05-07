namespace Marketing.Application;

public abstract record ProductException(string Message)
{
    public sealed record NotFound() : ProductException("Product not found");
    public sealed record AlreadyExists() : ProductException("Product already exists");
}