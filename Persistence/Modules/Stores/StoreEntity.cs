using Domain;
using Persistence.Modules.Products.Entities;

namespace Persistence.Modules.Stores.Entities
{
    public class StoreEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<ProductEntity> Products { get; set; }
        public Rating Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? EditedAt { get; set; }
    }
}