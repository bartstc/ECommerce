namespace Application.Products.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public MoneyDto Price { get; set; }
        public RatingDto Rating { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime? EditedAt { get; set; }
    }


    public class MoneyDto
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }

    public class RatingDto
    {
        public double Rate { get; set; }
        public int Count { get; set; }
    }
}