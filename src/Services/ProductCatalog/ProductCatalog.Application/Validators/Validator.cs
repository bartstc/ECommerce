namespace Application.Products.Validators;

public static class Validator
{
    public static bool BeValidCategory(string category)
    {
        return category switch
        {
            "clothing" => true,
            "jewelery" => true,
            "electronics" => true,
            _ => false
        };
    }

    public static bool BeValidCurrencyCode(string category)
    {
        return category switch
        {
            "USD" => true,
            "CAD" => true,
            "EUR" => true,
            _ => false
        };
    }
}