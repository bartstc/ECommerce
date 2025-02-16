using Domain;
using Persistence.Projections;

namespace API.Products.Dtos;

public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    MoneyDto Price,
    RatingDto Rating,
    string ImageUrl,
    string Category,
    DateTime AddedAt,
    DateTime? UpdatedAt
);

public record MoneyDto(
    decimal Amount,
    string Currency
);

public record RatingDto(
    double Rate,
    int Count
);

public static class ProductDtoMapper
{
    public static ProductDto ToDto(this ProductDetails product)
    {
        return new ProductDto(
            Id: product.Id,
            Name: product.Name,
            Description: product.Description,
            Price: new MoneyDto(
                Amount: product.PriceAmount,
                Currency: product.PriceCode
            ),
            Rating: new RatingDto(
                Rate: Math.Round(product.RatingRate, 2),
                Count: product.RatingCount
            ),
            ImageUrl: product.ImageUrl,
            Category: MapCategoryToString(product.Category),
            AddedAt: product.AddedAt,
            UpdatedAt: product.UpdatedAt
        );
    }



    private static string MapCategoryToString(Category category)
    {
        return category switch
        {
            Category.Clothing => "clothing",
            Category.Jewelery => "jewelery",
            Category.Electronics => "electronics",
            _ => throw new ArgumentException("Invalid category value")
        };
    }
}