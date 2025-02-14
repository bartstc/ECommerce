using Application.Products.Dtos;
using Domain;

namespace Application.Products.Mappers
{
    public static class ProductMapper
    {
        public static ProductDto ToDto(this Product product)
        {
            return new ProductDto(
                Id: product.Id.Value,
                Name: product.Name,
                Description: product.Description,
                Price: new MoneyDto(
                    Amount: product.Price.Amount,
                    Currency: product.Price.Currency.Code
                ),
                Rating: new RatingDto(
                    Rate: Math.Round(product.Rating.Rate, 2),
                    Count: product.Rating.Count
                ),
                ImageUrl: product.ImageUrl,
                Category: MapCategoryToString(product.Category),
                AddedAt: product.AddedAt,
                EditedAt: product.EditedAt
            );
        }

        public static Product ToDomain(this CreateProductDto productDto)
        {
            return Product.Create(new ProductData(
                null,
                productDto.Name,
                productDto.Description,
                Money.Of(productDto.Price.Amount, productDto.Price.Code),
                Rating.Of(productDto.Rating.Rate, productDto.Rating.Count),
                productDto.ImageUrl,
                MapStringToCategory(productDto.Category),
                DateTime.UtcNow,
                null
            ));
        }

        public static Product ToDomain(this CreateProductDto productDto, Product product)
        {
            return Product.Create(new ProductData(
                product.Id.Value,
                productDto.Name,
                productDto.Description,
                Money.Of(productDto.Price.Amount, productDto.Price.Code),
                Rating.Of(productDto.Rating.Rate, productDto.Rating.Count),
                productDto.ImageUrl,
                MapStringToCategory(productDto.Category),
                product.AddedAt,
                product.EditedAt
            ));
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