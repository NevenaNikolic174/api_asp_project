using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyProject.Infrastructure.Data.Configurations
{
    public class CartConfig : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {

            builder.Property(c => c.Quantity).IsRequired();

            builder.HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");

        }
    }
}
