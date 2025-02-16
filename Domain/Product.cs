using Ecommerce.Core.Domain;
using ECommerce.Core.Exceptions;

namespace Domain;

public class Product : AggregateRoot<ProductId>
{
    public string Name { get; private set; }
    public Category Category { get; private set; }
    public string Description { get; private set; }
    public string ImageUrl { get; private set; }
    public Money Price { get; private set; }
    public Rating Rating { get; private set; }
    public ProductStatus Status { get; private set; }
    public DateTime AddedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public static Product Create(ProductData productData)
    {
        var (Name, Description, Price, ImageUrl, Category) = productData
            ?? throw new ArgumentNullException(nameof(productData));

        if (string.IsNullOrWhiteSpace(Name))
            throw new BusinessRuleException("Product name cannot be null or whitespace.");
        if (string.IsNullOrWhiteSpace(Category.ToString()))
            throw new BusinessRuleException("Product category cannot be null or whitespace.");
        if (string.IsNullOrWhiteSpace(Description))
            throw new BusinessRuleException("Product description cannot be null or whitespace.");
        if (string.IsNullOrWhiteSpace(ImageUrl))
            throw new BusinessRuleException("ImageUrl cannot be null.");
        if (Price is null)
            throw new BusinessRuleException("Product price cannot be null.");

        return new Product(productData);
    }

    public void Update(ProductData productData)
    {
        if (Status != ProductStatus.Active)
            throw new BusinessRuleException($"Product cannot be updated when '{Status}'");

        var @event = new ProductEvent.ProductUpdated(
            Id.Value,
            productData.Name,
            productData.Description,
            productData.Price.Amount,
            productData.Price.Currency.Code,
            productData.ImageUrl,
            productData.Category);

        AppendEvent(@event);
        Apply(@event);
    }

    public void Rate(double rating)
    {
        if (Status != ProductStatus.Active)
            throw new BusinessRuleException($"Product cannot be rated when '{Status}'");

        var @event = new ProductEvent.ProductRated(Id.Value, rating);
        AppendEvent(@event);
        Apply(@event);
    }

    public void Delete()
    {
        if (Status == ProductStatus.Deleted)
            throw new BusinessRuleException("Product is already deleted.");

        var @event = new ProductEvent.ProductDeleted(Id.Value);
        AppendEvent(@event);
        Apply(@event);
    }

    private void Apply(ProductEvent.ProductAdded @event)
    {
        Id = ProductId.Of(@event.ProductId);
        Rating = Rating.Of(0, 0);
        Status = ProductStatus.Active;
        Name = @event.Name;
        Category = @event.Category;
        Description = @event.Description;
        ImageUrl = @event.ImageUrl;
        Price = Money.Of(@event.PriceAmount, @event.PriceCode);
        Rating = Rating.Of(0, 0);
        AddedAt = @event.Timestamp;
    }

    private void Apply(ProductEvent.ProductUpdated @event)
    {
        Name = @event.Name;
        Category = @event.Category;
        Description = @event.Description;
        ImageUrl = @event.ImageUrl;
        Price = Money.Of(@event.PriceAmount, @event.PriceCode);
        UpdatedAt = @event.Timestamp;
    }

    private void Apply(ProductEvent.ProductRated @event)
    {
        Rating = Rating.Recalculate(@event.Rating);
    }

    private void Apply(ProductEvent.ProductDeleted @event)
    {
        Status = ProductStatus.Deleted;
        DeletedAt = @event.Timestamp;
    }

    private Product(ProductData productData)
    {
        var productId = Guid.NewGuid();
        var @event = new ProductEvent.ProductAdded(
            productId,
            productData.Name,
            productData.Description,
            productData.Price.Amount,
            productData.Price.Currency.Code,
            productData.ImageUrl,
            productData.Category);

        AppendEvent(@event);
        Apply(@event);
    }

    private Product() { }
}