using Domain.Events;
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
    public DateTime AddedAt { get; private set; }
    // public DateTime? EditedAt { get; private set; }

    public static Product Create(ProductData productData)
    {
        var (Name, Description, Price, Rating, ImageUrl, Category) = productData
            ?? throw new ArgumentNullException(nameof(productData));

        if (string.IsNullOrWhiteSpace(Name))
            throw new BusinessRuleException("Product name cannot be null or whitespace.");
        if (string.IsNullOrWhiteSpace(Category.ToString()))
            throw new BusinessRuleException("Product category cannot be null or whitespace.");
        if (string.IsNullOrWhiteSpace(Description))
            throw new BusinessRuleException("Product description cannot be null or whitespace.");
        if (string.IsNullOrWhiteSpace(ImageUrl))
            throw new BusinessRuleException("ImageUrl cannot be null.");
        if (Rating is null)
            throw new BusinessRuleException("Product rating cannot be null.");
        if (Price is null)
            throw new BusinessRuleException("Product price cannot be null.");

        return new Product(productData);
    }

    private void Apply(ProductAdded @event)
    {
        Id = ProductId.Of(@event.ProductId);
        Name = @event.ProductData.Name;
        Category = @event.ProductData.Category;
        Description = @event.ProductData.Description;
        ImageUrl = @event.ProductData.ImageUrl;
        Price = @event.ProductData.Price;
        Rating = @event.ProductData.Rating;
        AddedAt = @event.Timestamp;
    }

    private Product(ProductData productData)
    {
        var @event = ProductAdded.Create(Guid.NewGuid(), productData);
        AppendEvent(@event);
        Apply(@event);
    }

    // public void Update(ProductData productData)
    // {
    //     Name = productData.Name;
    //     Category = productData.Category;
    //     Description = productData.Description;
    //     ImageUrl = productData.ImageUrl;
    //     Price = productData.Price;
    //     Rating = productData.Rating;
    //     EditedAt = DateTime.UtcNow;
    // }

    private Product() { }

    public void RateProduct(double rate)
    {
        Rating = Rating.UpdateRating(rate);
    }
}