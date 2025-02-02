using API.Dtos;
using Domain;

namespace API.Mappers
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
    }
}