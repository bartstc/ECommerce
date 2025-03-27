namespace ProductCatalog.Domain;

public record class ProductData(
    string Name,
    string Description,
    Money Price,
    string ImageUrl,
    Category Category,
    ProductId? ProductId = null // for seed purposes
);