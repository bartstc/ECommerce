using Marketing.Infrastructure.Projections;
using Swashbuckle.AspNetCore.Annotations;

namespace Marketing.API.Products.Dtos;

[SwaggerSchema("Product details")]
public record ProductDto(
    [property: SwaggerSchema("Product ID")] Guid Id,
    [property: SwaggerSchema("Product rating")] RatingDto Rating,
    [property: SwaggerSchema("Date when the product was added")] DateTime AddedAt,
    [property: SwaggerSchema("Date when the product was last updated")] DateTime? UpdatedAt
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
            Rating: new RatingDto(
                Rate: Math.Round(product.RatingRate, 2),
                Count: product.RatingCount
            ),
            AddedAt: product.AddedAt,
            UpdatedAt: product.UpdatedAt
        );
    }
}