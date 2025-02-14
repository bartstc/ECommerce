using Domain;

namespace Persistence.Modules.Products.Mappers
{
    public static class ProductMapper
    {
        public static Product ToDomain(this Product product)
        {
            return Product.Create(new ProductData(
                product.Id.Value,
                product.Name,
                product.Description,
                product.Price,
                product.Rating,
                product.ImageUrl,
                product.Category,
                product.AddedAt,
                product.EditedAt
            ));
        }

        public static Product ToPersistence(this Product product)
        {
            return Product.Create(new ProductData(
                product.Id.Value,
                product.Name,
                product.Description,
                product.Price,
                product.Rating,
                product.ImageUrl,
                product.Category,
                product.AddedAt,
                product.EditedAt
            ));
        }
    }
}