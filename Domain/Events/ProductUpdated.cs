using EcommerceDDD.Core.Domain;

namespace Domain.Events;

public record class ProductUpdated : DomainEvent
{
    public Guid ProductId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal PriceAmount { get; private set; }
    public string PriceCode { get; private set; }
    public string ImageUrl { get; private set; }
    public Category Category { get; private set; }

    private ProductUpdated(
        Guid productId,
        string name,
        string description,
        decimal priceAmount,
        string priceCode,
        string imageUrl,
        Category category)
    {
        ProductId = productId;
        Name = name;
        Description = description;
        PriceAmount = priceAmount;
        PriceCode = priceCode;
        ImageUrl = imageUrl;
        Category = category;
    }

    public static ProductUpdated Create(
        Guid productId,
        string name,
        string description,
        decimal priceAmount,
        string priceCode,
        string imageUrl,
        Category category)
    {
        if (productId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(productId));
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrEmpty(description))
            throw new ArgumentNullException(nameof(description));
        if (priceAmount <= 0)
            throw new ArgumentOutOfRangeException(nameof(priceAmount));
        if (string.IsNullOrEmpty(priceCode))
            throw new ArgumentNullException(nameof(priceCode));
        if (string.IsNullOrEmpty(imageUrl))
            throw new ArgumentNullException(nameof(imageUrl));

        return new ProductUpdated(
            productId,
            name,
            description,
            priceAmount,
            priceCode,
            imageUrl,
            category
        );
    }
}

