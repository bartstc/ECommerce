namespace ProductCatalog.Domain;

public record class ProductData(
    Money Price,
    Category Category,
    ProductId? ProductId = null // for seed purposes
);