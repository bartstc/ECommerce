namespace Domain
{
    public class Store
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Product> Products { get; set; }
        public Rating Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? EditedAt { get; set; }

        public void RecalculateRating(double rate)
        {
            Rating = Rating.UpdateRating(rate);
        }
    }
}