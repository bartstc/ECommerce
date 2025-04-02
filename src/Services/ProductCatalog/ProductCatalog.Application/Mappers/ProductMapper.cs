using ProductCatalog.Application.Products.Dtos;

namespace ProductCatalog.Application.Products.Mappers;

public static class ProductMapper
{
    public static ProductData ToProductData(this AddProductDto productDto)
    {
        return new ProductData(
            Money.Of(productDto.Price.Amount, productDto.Price.Code),
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