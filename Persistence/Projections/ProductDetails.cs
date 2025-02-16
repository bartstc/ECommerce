using Domain;

namespace Persistence.Projections;

public class ProductDetails
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Category Category { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public decimal PriceAmount { get; set; }
    public string PriceCode { get; set; }
    public double RatingRate { get; set; }
    public int RatingCount { get; set; }
    public ProductStatus Status { get; set; }
    public DateTime AddedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    internal void Apply(ProductEvent.ProductAdded @event)
    {
        Id = @event.ProductId;
        Name = @event.Name;
        Category = @event.Category;
        Description = @event.Description;
        ImageUrl = @event.ImageUrl;
        PriceAmount = @event.PriceAmount;
        PriceCode = @event.PriceCode;
        AddedAt = @event.Timestamp;
        Status = ProductStatus.Active;
    }

    internal void Apply(ProductEvent.ProductUpdated @event)
    {
        Name = @event.Name;
        Description = @event.Description;
        PriceAmount = @event.PriceAmount;
        PriceCode = @event.PriceCode;
        ImageUrl = @event.ImageUrl;
        Category = @event.Category;
        UpdatedAt = @event.Timestamp;
    }

    internal void Apply(ProductEvent.ProductRated @event)
    {
        var rating = Rating.Of(RatingRate, RatingCount);
        var newRating = rating.Recalculate(@event.Rating);
        RatingRate = newRating.Rate;
        RatingCount = newRating.Count;
    }

    internal void Apply(ProductEvent.ProductDeleted @event)
    {
        Status = ProductStatus.Deleted;
        DeletedAt = @event.Timestamp;
    }
}
