using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser> // DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Store> Stores { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title);
                entity.Property(e => e.Description);
                entity.OwnsOne(e => e.Price, price =>
                {
                    price.Property(p => p.Amount).HasColumnName("PriceAmount");
                    price.Property(p => p.Currency).HasColumnName("PriceCurrency").HasConversion<string>();
                });
                entity.OwnsOne(e => e.Rating, rating =>
                {
                    rating.Property(p => p.Rate).HasColumnName("RatingRate");
                    rating.Property(p => p.Count).HasColumnName("RatingCount");
                });
                entity.Property(e => e.Image);
                entity.Property(e => e.Category).HasConversion<string>();
                entity.Property(e => e.AddedAt);
                entity.Property(e => e.EditedAt);

                entity.HasOne(e => e.Store)
                    .WithMany(s => s.Products)
                    .HasForeignKey(e => e.StoreId);
            });

            // no need to configure the reverse relationship with Products, EF will understand it
            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name);
                entity.Property(e => e.Description);
                entity.Property(e => e.CreatedAt);
                entity.Property(e => e.EditedAt);
            });
        }
    }
}