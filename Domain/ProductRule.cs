using ECommerce.Core.Domain;

namespace Domain;

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
}