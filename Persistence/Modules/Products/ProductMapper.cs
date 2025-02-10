using Domain;
using Persistence.Modules.Products.Entities;

namespace Persistence.Modules.Products.Mappers
{
    public static class ProductMapper
    {
        // public static Product Create(this ProductEntity productEntity)
        // {
        //     return new Product
        //     {
        //         Id = Guid.NewGuid(),
        //         Title = productEntity.Title,
        //         Description = productEntity.Description,
        //         Price = new Money(productEntity.Price.Amount, productEntity.Price.Currency),
        //         Rating = new Rating(productEntity.Rating.Rate, productEntity.Rating.Count),
        //         Image = productEntity.Image,
        //         Category = productEntity.Category,
        //         // temporary, will be provided from the user's metchant profile's store
        //         StoreId = Guid.Parse("c4f297c6-dd1a-44ad-bf41-428ac0310a62"),
        //         AddedAt = DateTime.UtcNow
        //     };
        // }

        public static Product ToDomain(this ProductEntity productEntity)
        {
            return new Product
            {
                Id = productEntity.Id,
                Title = productEntity.Title,
                Description = productEntity.Description,
                Price = new Money(productEntity.Price.Amount, productEntity.Price.Currency),
                Rating = new Rating(productEntity.Rating.Rate, productEntity.Rating.Count),
                Image = productEntity.Image,
                Category = productEntity.Category,
                StoreId = productEntity.StoreId,
                AddedAt = productEntity.AddedAt,
                EditedAt = productEntity.EditedAt
            };
        }

        public static ProductEntity ToPersistence(this Product product)
        {
            return new ProductEntity
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = new Money(product.Price.Amount, product.Price.Currency),
                Rating = new Rating(product.Rating.Rate, product.Rating.Count),
                Image = product.Image,
                Category = product.Category,
                StoreId = product.StoreId,
                AddedAt = product.AddedAt,
                EditedAt = product.EditedAt
            };
        }
    }
}