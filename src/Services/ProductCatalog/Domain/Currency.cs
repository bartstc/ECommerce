using Ecommerce.Core.Domain;
using ECommerce.Core.Exceptions;

namespace ProductCatalog.Domain;

public class Currency : ValueObject<Currency>
{
    public string Code { get; }
    public string Symbol { get; }
    public static Currency USDollar => new Currency("USD", "$");
    public static Currency CanadianDollar => new Currency("CAD", "CA$");
    public static Currency Euro => new Currency("EUR", "€");

    public static Currency OfCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new BusinessValidationException("Code cannot be null or whitespace.");

        return code switch
        {
            "USD" => new Currency(USDollar.Code, USDollar.Symbol),
            "CAD" => new Currency(CanadianDollar.Code, CanadianDollar.Symbol),
            "EUR" => new Currency(Euro.Code, Euro.Symbol),
            _ => throw new BusinessValidationException($"Invalid code {code}")
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
        yield return Symbol;
    }

    private Currency(string code, string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new BusinessValidationException("Symbol cannot be null or whitespace.");

        Code = code;
        Symbol = symbol;
    }

    private Currency() { }
}