using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyProject.Domain.Entities;

namespace MyProject.DataAccess.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(15);
            builder.HasIndex(x => x.Username).IsUnique();

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(50);
            builder.HasIndex(x => x.Email).IsUnique();

            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.FirstName).IsRequired();
            builder.Property(x => x.LastName).IsRequired();

            builder.HasMany(u => u.Carts)
               .WithOne(c => c.User)
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Orders)
              .WithOne(c => c.User)
              .HasForeignKey(c => c.UserId)
              .OnDelete(DeleteBehavior.Cascade);



            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
