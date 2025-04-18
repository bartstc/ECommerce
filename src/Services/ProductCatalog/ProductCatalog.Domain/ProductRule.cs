using ECommerce.Core.Domain;

namespace ProductCatalog.Domain;

public abstract record ProductRule
{
    public record ProductMustBeActiveRule(ProductStatus status) : IBusinessRule
    {
        public string Message => $"Operation not allowed when in '{status}' status";

        public bool IsBroken()
        {
            return status != ProductStatus.Active;
        }
    }

    public record ProductDataIsValidRule(ProductData productData) : IBusinessRule
    {
        public string Message => "Product data is invalid";

        public bool IsBroken()
        {
            return productData is null
                   || string.IsNullOrWhiteSpace(productData.Category.ToString())
                   || productData.Price is null;
        }
    }

    public record PriceIsValidRule(Money price) : IBusinessRule
    {
        public string Message => "Price is invalid";

        public bool IsBroken()
        {
            return price is null;
        }
    }
}