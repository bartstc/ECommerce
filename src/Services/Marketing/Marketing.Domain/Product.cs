namespace Marketing.Domain;

public class Product : AggregateRoot<ProductId>
{
    public ProductStatus Status { get; private set; }
    public Rating Rating { get; private set; }
    public DateTime AddedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public static Product Create(ProductData productData)
    {
        CheckRule(new ProductRule.ProductDataIsValidRule(productData));

        return new Product(productData);
    }

    public void Rate(double rating)
    {
        CheckRule(new ProductRule.ProductMustBeActiveRule(Status));

        var @event = new ProductEvent.ProductRated(Id.Value, rating);
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

    private void Apply(ProductEvent.ProductCreated @event)
    {
        Id = ProductId.Of(@event.ProductId);
        Status = ProductStatus.Active;
        Rating = Rating.Of(0, 0);
        AddedAt = @event.Timestamp;
    }

    private void Apply(ProductEvent.ProductRated @event)
    {
        Rating = Rating.Recalculate(@event.Rating);
        UpdatedAt = @event.Timestamp;
    }

    private void Apply(ProductEvent.ProductDeleted @event)
    {
        Status = ProductStatus.Deleted;
        DeletedAt = @event.Timestamp;
    }

    private Product(ProductData productData)
    {
        var productId = Guid.NewGuid();
        var @event = new ProductEvent.ProductCreated(productId);

        AppendEvent(@event);
        Apply(@event);
    }

    private Product() { }
}