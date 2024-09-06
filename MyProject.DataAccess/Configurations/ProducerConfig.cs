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
    public class ProducerConfig : IEntityTypeConfiguration<Producer>
    {
        public void Configure(EntityTypeBuilder<Producer> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.Name).IsUnique();
            builder.Property(x => x.Phone).HasMaxLength(12);

            builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");


        }
    }
}
