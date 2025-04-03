namespace ProductCatalog.Domain;

public class Product : AggregateRoot<ProductId>
{
    public Category Category { get; private set; }
    public Money Price { get; private set; }
    public ProductStatus Status { get; private set; }
    public DateTime AddedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public static Product Create(ProductData productData)
    {
        CheckRule(new ProductRule.ProductDataIsValidRule(productData));

        return new Product(productData);
    }

    public void UpdatePrice(Money price)
    {
        CheckRule(new ProductRule.ProductMustBeActiveRule(Status));
        CheckRule(new ProductRule.PriceIsValidRule(price));

        var @event = new ProductEvent.PriceUpdated(
            Id.Value,
            price.Amount,
            Price.Currency.Code);

        AppendEvent(@event);
        Apply(@event);
    }

    public void Delete()
    {
        CheckRule(new ProductRule.ProductMustBeActiveRule(Status));

        var @event = new ProductEvent.ProductDeleted(Id.Value);
        AppendEvent(@event);
        Apply(@event);
    }

    private void Apply(ProductEvent.ProductAdded @event)
    {
        Id = ProductId.Of(@event.ProductId);
        Status = ProductStatus.Active;
        Category = @event.Category;
        Price = Money.Of(@event.PriceAmount, @event.PriceCode);
        AddedAt = @event.Timestamp;
    }

    private void Apply(ProductEvent.PriceUpdated @event)
    {
        Price = Money.Of(@event.PriceAmount, @event.PriceCode);
        UpdatedAt = @event.Timestamp;
    }

    private void Apply(ProductEvent.ProductDeleted @event)
    {
        Status = ProductStatus.Deleted;
        DeletedAt = @event.Timestamp;
    }

    private Product(ProductData productData)
    {
        var productId = productData.ProductId?.Value ?? Guid.NewGuid();
        var @event = new ProductEvent.ProductAdded(
            productId,
            productData.Price.Amount,
            productData.Price.Currency.Code,
            productData.Category);

        AppendEvent(@event);
        Apply(@event);
    }

    private Product()
    {
    }
}