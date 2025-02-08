namespace Application.Products.Dtos
{
    public class CreateProductDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public CreatePriceDto Price { get; set; }
        public CreateRatingDto Rating { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
    }

    public class CreatePriceDto
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }

    public class CreateRatingDto
    {
        public double Rate { get; set; }
        public int Count { get; set; }
    }
}