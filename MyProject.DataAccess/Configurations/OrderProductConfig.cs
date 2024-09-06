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
    public class OrderProductConfig : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.HasKey(op => new { op.OrderId, op.ProductId });

            builder.HasOne(op => op.Product)
                   .WithMany(p => p.OrderProducts)
                   .HasForeignKey(op => op.ProductId);

            builder.HasOne(op => op.Order)
                   .WithMany(o => o.OrderProducts)
                   .HasForeignKey(o => o.OrderId);


        }
    }
}
