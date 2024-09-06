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
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(30);
            builder.HasIndex(x => x.Name).IsUnique();

            builder.Property(x => x.ProductCode).IsRequired().HasMaxLength(5);
            builder.HasIndex(x => x.ProductCode).IsUnique();

            builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.Price)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)")
                   .HasDefaultValue(0)
                   .HasPrecision(18, 2);

            builder.HasOne(c => c.Category)
                   .WithMany(p => p.Products)
                   .HasForeignKey(x => x.CategoryId)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired();

            builder.HasOne(p => p.Producer)
                   .WithMany(pr => pr.Products)
                   .HasForeignKey(x => x.ProducerId)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired();

            builder.HasOne(s => s.Status)
                   .WithMany(p => p.Products)
                   .HasForeignKey(x => x.StatusId)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired();

            builder.HasMany(pr => pr.ProductRestaurants)
                   .WithOne(p => p.Product)
                   .HasForeignKey(x => x.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Carts)
                  .WithOne(p => p.Product)
                  .HasForeignKey(x => x.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);

           
        }
    }
}
