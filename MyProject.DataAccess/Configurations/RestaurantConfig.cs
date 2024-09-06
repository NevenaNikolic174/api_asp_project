using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.DataAccess.Configurations
{
    public class RestaurantConfig : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(20);
            builder.HasIndex(x => x.Name).IsUnique();

            builder.Property(x => x.Address).IsRequired().HasMaxLength(50);

            builder.HasMany(pr => pr.ProductRestaurants)
                   .WithOne(r => r.Restaurant)
                   .HasForeignKey(x => x.RestaurantId)
                   .OnDelete(DeleteBehavior.Cascade);


            builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");

        }
    }
}
