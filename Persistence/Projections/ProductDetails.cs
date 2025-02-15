using Domain;
using Domain.Events;

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

    internal void Apply(ProductAdded @event)
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

    internal void Apply(ProductUpdated @event)
    {
        Name = @event.Name;
        Description = @event.Description;
        PriceAmount = @event.PriceAmount;
        PriceCode = @event.PriceCode;
        ImageUrl = @event.ImageUrl;
        Category = @event.Category;
    }

    internal void Apply(ProductRated @event)
    {
        var rating = Rating.Of(RatingRate, RatingCount);
        var newRating = rating.Recalculate(@event.Rating);
        RatingRate = newRating.Rate;
        RatingCount = newRating.Count;
    }

    internal void Apply(ProductDeleted @event)
    {
        Status = ProductStatus.Deleted;
    }
}
