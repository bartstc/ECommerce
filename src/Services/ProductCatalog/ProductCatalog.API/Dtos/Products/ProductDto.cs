using ProductCatalog.Domain;
using ProductCatalog.Infrastructure.Documents;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductCatalog.API.Products.Dtos;

[SwaggerSchema("Product details")]
public record ProductDto(
    [property: SwaggerSchema("Product ID")] Guid Id,
    [property: SwaggerSchema("Product name")] string Name,
    [property: SwaggerSchema("Product description")] string Description,
    [property: SwaggerSchema("Product price")] MoneyDto Price,
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

public static class ProductDtoMapper
{
    public static ProductDto ToDto(this ProductDocument product)
    {
        return new ProductDto(
            Id: product.ProductId,
            Name: product.Name,
            Description: product.Description,
            Price: new MoneyDto(
                Amount: product.PriceAmount,
                Currency: product.PriceCode
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