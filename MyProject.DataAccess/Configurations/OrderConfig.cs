using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyProject.Infrastructure.Data.Configurations
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(c => c.User)
                 .WithMany(u => u.Orders)
                 .HasForeignKey(c => c.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.Payment)
                .WithMany()
                .HasForeignKey(o => o.PaymentId);

            builder.Property(o => o.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.CreditCardNumber)
                .IsRequired(false);

            builder.Property(o => o.UserAddress)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");

        }
    }
}
