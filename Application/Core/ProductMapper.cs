using Application.Dtos;
using Domain;

namespace Application.Mappers
{
    public static class ProductMapper
    {
        public static ProductDto ToDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = new MoneyDto
                {
                    Amount = product.Price.Amount,
                    Currency = product.Price.Currency.ToString()
                },
                Rating = new RatingDto
                {
                    Rate = product.Rating.Rate,
                    Count = product.Rating.Count
                },
                Image = product.Image,
                Category = product.Category switch
                {
                    Category.MensClothing => "men's clothing",
                    Category.WomensClothing => "women's clothing",
                    Category.Jewelery => "jewelery",
                    Category.Electronics => "electronics",
                    _ => throw new ArgumentException("Invalid category value")
                },
                AddedAt = product.AddedAt,
                EditedAt = product.EditedAt
            };
        }

        public static Product ToDomain(this CreateProductDto productDto)
        {
            return new Product
            {
                Id = Guid.NewGuid(),
                Title = productDto.Title,
                Description = productDto.Description,
                Price = new Money(productDto.Price.Amount, Enum.Parse<Currency>(productDto.Price.Currency)),
                Rating = new Rating(productDto.Rating.Rate, productDto.Rating.Count),
                Image = productDto.Image,
                Category = productDto.Category switch
                {
                    "men's clothing" => Category.MensClothing,
                    "women's clothing" => Category.WomensClothing,
                    "jewelery" => Category.Jewelery,
                    "electronics" => Category.Electronics,
                    _ => throw new ArgumentException("Invalid category value")
                },
                AddedAt = DateTime.UtcNow
            };
        }

        public static Product ToDomain(this CreateProductDto productDto, Product product)
        {
            return new Product
            {
                Id = product.Id,
                Title = productDto.Title,
                Description = productDto.Description,
                Price = new Money(productDto.Price.Amount, Enum.Parse<Currency>(productDto.Price.Currency)),
                Rating = new Rating(productDto.Rating.Rate, productDto.Rating.Count),
                Image = productDto.Image,
                Category = productDto.Category switch
                {
                    "men's clothing" => Category.MensClothing,
                    "women's clothing" => Category.WomensClothing,
                    "jewelery" => Category.Jewelery,
                    "electronics" => Category.Electronics,
                    _ => throw new ArgumentException("Invalid category value")
                },
                AddedAt = product.AddedAt,
                EditedAt = DateTime.UtcNow
            };
        }
    }
}