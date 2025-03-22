using ProductCatalog.Application.Products.Dtos;
using ProductCatalog.Domain;

namespace ProductCatalog.Application.Products.Mappers;

public static class ProductMapper
{
    public static ProductData ToProductData(this CreateProductDto productDto)
    {
        return new ProductData(
            productDto.Name,
            productDto.Description,
            Money.Of(productDto.Price.Amount, productDto.Price.Code),
            productDto.ImageUrl,
            MapStringToCategory(productDto.Category)
        );
    }

    private static Category MapStringToCategory(string category)
    {
        return category switch
        {
            "clothing" => Category.Clothing,
            "jewelery" => Category.Jewelery,
            "electronics" => Category.Electronics,
            _ => throw new ArgumentException("Invalid category value")
        };
    }
}
