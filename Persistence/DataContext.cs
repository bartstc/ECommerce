using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser> // DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(x => x.Id)
                    .HasConversion(
                        v => v.Value,
                        v => ProductId.Of(v));

                entity.Ignore(t => t.Version);

                entity.Property(e => e.Name);
                entity.Property(e => e.Description);
                entity.OwnsOne(e => e.Price, price =>
                {
                    price.Property(p => p.Amount).HasColumnName("Price");
                    price.OwnsOne(p => p.Currency, currency =>
                    {
                        currency.Property(e => e.Code)
                            .HasColumnName("CurrencyCode")
                            .HasMaxLength(5).IsRequired();

                        currency.Property(e => e.Symbol)
                            .HasColumnName("CurrencySymbol")
                            .HasMaxLength(5);
                    });
                });
                entity.OwnsOne(e => e.Rating, rating =>
                {
                    rating.Property(p => p.Rate).HasColumnName("RatingRate");
                    rating.Property(p => p.Count).HasColumnName("RatingCount");
                });
                entity.Property(e => e.ImageUrl);
                entity.Property(e => e.Category).HasConversion<string>();
                entity.Property(e => e.AddedAt);
                entity.Property(e => e.EditedAt);
            });
        }
    }
}