using ProductCatalog.Domain;
using ProductCatalog.Infrastructure.Projections;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductCatalog.API.Products.Dtos;

[SwaggerSchema("Product details")]
public record ProductDto(
    [property: SwaggerSchema("Product ID")] Guid Id,
    [property: SwaggerSchema("Product name")] string Name,
    [property: SwaggerSchema("Product description")] string Description,
    [property: SwaggerSchema("Product price")] MoneyDto Price,
    [property: SwaggerSchema("Product rating")] RatingDto Rating,
    [property: SwaggerSchema("Product image URL")] string ImageUrl,
    [property: SwaggerSchema("Product category")] string Category,
    [property: SwaggerSchema("Date when the product was added")] DateTime AddedAt,
    [property: SwaggerSchema("Date when the product was last updated")] DateTime? UpdatedAt
);

[SwaggerSchema("Monetary value")]
public record MoneyDto(
    [property: SwaggerSchema("Amount of money")] decimal Amount,
    [property: SwaggerSchema("Currency code")] string Currency
);

[SwaggerSchema("Product rating details")]
public record RatingDto(
    [property: SwaggerSchema("Rating value")] double Rate,
    [property: SwaggerSchema("Number of ratings")] int Count
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