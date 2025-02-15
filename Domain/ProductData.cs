namespace Domain;

public record class ProductData(
    string Name,
    string Description,
    Money Price,
    Rating Rating,
    string ImageUrl,
    Category Category
);