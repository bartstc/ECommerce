using Application.Products.Dtos;
using Domain;
using Persistence.Projections;

namespace Application.Products.Mappers
{
    public static class ProductMapper
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
}