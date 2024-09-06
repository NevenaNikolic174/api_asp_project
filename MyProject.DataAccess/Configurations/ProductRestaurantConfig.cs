using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyProject.Domain.Entities;

namespace MyProject.DataAccess.Configurations
{
    public class ProductRestaurantConfig : IEntityTypeConfiguration<ProductRestaurant>
    {
        public void Configure(EntityTypeBuilder<ProductRestaurant> builder)
        {
            builder.HasKey(pr => new { pr.ProductId, pr.RestaurantId });

            builder.HasOne(pr => pr.Product)
                   .WithMany(p => p.ProductRestaurants)
                   .HasForeignKey(pr => pr.ProductId);

            builder.HasOne(pr => pr.Restaurant)
                   .WithMany(r => r.ProductRestaurants)
                   .HasForeignKey(pr => pr.RestaurantId);
        }
    }
}
