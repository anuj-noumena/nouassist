using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Infrastructure.Activity.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Infrastructure.Activity.EntityFrameworkCore.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(ActivityDbProperties.DbTablePrefix + "AuditLogs", 
                           ActivityDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId).IsRequired();
            
            builder.Property(x => x.UserName)
                .HasMaxLength(256);

            builder.Property(x => x.Action)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Category)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Severity)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.EntityType)
                .HasMaxLength(256);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.IpAddress)
                .HasMaxLength(64);

            builder.Property(x => x.UserAgent)
                .HasMaxLength(500);

            builder.Property(x => x.ErrorMessage)
                .HasMaxLength(4000);

            // JSON columns
            builder.Property(x => x.Metadata)
                .HasColumnType("jsonb");

            builder.Property(x => x.ChangedFields)
                .HasColumnType("jsonb");

            // Indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.Category);
            builder.HasIndex(x => x.Severity);
            builder.HasIndex(x => x.Timestamp);
            builder.HasIndex(x => x.TenantId);
        }
    }
}
