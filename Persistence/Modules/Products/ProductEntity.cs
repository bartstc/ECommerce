using Domain;

namespace Persistence.Modules.Products.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Money Price { get; set; }
        public Rating Rating { get; set; }
        public string Image { get; set; }
        public Category Category { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime? EditedAt { get; set; }
    }
}