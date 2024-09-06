using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyProject.DataAccess.Configurations
{
    public class AuditLogConfig : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.Property(x => x.UserName).IsRequired();
            builder.Property(x => x.UseCaseName).IsRequired();
            builder.Property(x => x.ActionStatus).IsRequired();
            builder.Property(x => x.ExecutedAt).IsRequired().HasDefaultValueSql("GETDATE()");
        }
    }
}
