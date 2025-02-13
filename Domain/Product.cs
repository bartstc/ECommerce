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
    public DateTime? EditedAt { get; private set; }

    public static Product Create(ProductData productData)
    {
        var (Id, Name, Description, Price, Rating, ImageUrl, Category, AddedAt, EditedAt) = productData
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

    private Product(ProductData productData)
    {
        Id = ProductId.Of(productData.Id ?? Guid.NewGuid());
        Name = productData.Name;
        Category = productData.Category;
        Description = productData.Description;
        ImageUrl = productData.ImageUrl;
        Price = productData.Price;
        Rating = productData.Rating;
        AddedAt = productData.AddedAt;
        EditedAt = productData.EditedAt;
    }

    private Product() { }

    public void RateProduct(double rate)
    {
        Rating = Rating.UpdateRating(rate);
    }
}