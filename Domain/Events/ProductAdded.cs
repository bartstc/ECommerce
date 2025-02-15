using ECommerce.Core.Exceptions;
using EcommerceDDD.Core.Domain;

namespace Domain.Events;

public record class ProductAdded : DomainEvent
{
    public Guid ProductId { get; private set; }
    public ProductData ProductData { get; private set; }

    private ProductAdded(Guid productId, ProductData productData)
    {
        ProductId = productId;
        ProductData = productData;
    }

    public static ProductAdded Create(Guid productId, ProductData productData)
    {
        if (productId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(productId));

        if (productData is null)
            throw new ArgumentNullException("Product data cannot be null.");

        return new ProductAdded(productId, productData);
    }
}
