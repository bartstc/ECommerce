using Marten;

namespace Marketing.Infrastructure;

public class Seed
{
    public static async Task SeedData(IDocumentSession session)
    {
        var hasEvents = await session.Events.QueryAllRawEvents().AnyAsync();

        if (!hasEvents)
        {
            var products = new List<Product>
                {
                    Product.Create(new ProductData(ProductId.Of(Guid.Parse("4f968992-1aab-49c9-8913-09405915c1c0")))),
                    Product.Create(new ProductData(ProductId.Of(Guid.Parse("4f968992-1aab-49c9-8913-09405915c1c1")))),
                    Product.Create(new ProductData(ProductId.Of(Guid.Parse("4f968992-1aab-49c9-8913-09405915c1c2")))),
                    Product.Create(new ProductData(ProductId.Of(Guid.Parse("4f968992-1aab-49c9-8913-09405915c1c3")))),
                    Product.Create(new ProductData(ProductId.Of(Guid.Parse("4f968992-1aab-49c9-8913-09405915c1c4")))),
                    Product.Create(new ProductData(ProductId.Of(Guid.Parse("4f968992-1aab-49c9-8913-09405915c1c5")))),
                    Product.Create(new ProductData(ProductId.Of(Guid.Parse("4f968992-1aab-49c9-8913-09405915c1c6")))),
                    Product.Create(new ProductData(ProductId.Of(Guid.Parse("4f968992-1aab-49c9-8913-09405915c1c7")))),
                    Product.Create(new ProductData(ProductId.Of(Guid.Parse("4f968992-1aab-49c9-8913-09405915c1c8")))),
                    Product.Create(new ProductData(ProductId.Of(Guid.Parse("4f968992-1aab-49c9-8913-09405915c1c9")))),
                    Product.Create(new ProductData(ProductId.Of(Guid.Parse("4f968992-1aab-49c9-8913-09405915c1d0")))),
                    Product.Create(new ProductData(ProductId.Of(Guid.Parse("4f968992-1aab-49c9-8913-09405915c1c1")))),
                    Product.Create(new ProductData(ProductId.Of(Guid.Parse("4f968992-1aab-49c9-8913-09405915c1c2")))),
                    Product.Create(new ProductData(ProductId.Of(Guid.Parse("4f968992-1aab-49c9-8913-09405915c1c3")))),
                    Product.Create(new ProductData(ProductId.Of(Guid.Parse("4f968992-1aab-49c9-8913-09405915c1c4")))),
                    Product.Create(new ProductData(ProductId.Of(Guid.Parse("4f968992-1aab-49c9-8913-09405915c1c5")))),
                    Product.Create(new ProductData(ProductId.Of(Guid.Parse("4f968992-1aab-49c9-8913-09405915c1c6")))),
                    Product.Create(new ProductData(ProductId.Of(Guid.Parse("4f968992-1aab-49c9-8913-09405915c1c7")))),
                    Product.Create(new ProductData(ProductId.Of(Guid.Parse("4f968992-1aab-49c9-8913-09405915c1c8")))),
                    Product.Create(new ProductData(ProductId.Of(Guid.Parse("4f968992-1aab-49c9-8913-09405915c1c9"))))
                };

            foreach (var product in products)
            {
                var events = product.GetUncommittedEvents().ToArray();
                session.Events.Append(product.Id.Value, events);
                product.ClearUncommittedEvents();
            }

            await session.SaveChangesAsync();
        }
    }
}

