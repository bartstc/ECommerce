using Domain;
using Persistence.Modules.Products.Entities;

namespace Persistence.Modules.Products.Mappers
{
    public static class ProductMapper
    {
        public static Product ToDomain(this ProductEntity productEntity)
        {
            return Product.Create(new ProductData(
                productEntity.Id,
                productEntity.Name,
                productEntity.Description,
                Money.Of(productEntity.Price.Amount, productEntity.Price.Currency.Code),
                Rating.Of(productEntity.Rating.Rate, productEntity.Rating.Count),
                productEntity.ImageUrl,
                productEntity.Category,
                productEntity.AddedAt,
                productEntity.EditedAt
            ));
        }

        public static ProductEntity ToPersistence(this Product product)
        {
            return new ProductEntity
            {
                Id = product.Id.Value,
                Name = product.Name,
                Description = product.Description,
                Price = Money.Of(product.Price.Amount, product.Price.Currency.Code),
                Rating = Rating.Of(product.Rating.Rate, product.Rating.Count),
                ImageUrl = product.ImageUrl,
                Category = product.Category,
                AddedAt = product.AddedAt,
                EditedAt = product.EditedAt
            };
        }
    }
}