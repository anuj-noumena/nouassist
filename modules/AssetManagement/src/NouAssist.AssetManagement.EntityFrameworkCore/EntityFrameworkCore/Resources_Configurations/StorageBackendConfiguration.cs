using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.AssetManagement.Resources.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.AssetManagement.Resources.EntityFrameworkCore.Configurations
{
    public class StorageBackendConfiguration : IEntityTypeConfiguration<StorageBackend>
    {
        public void Configure(EntityTypeBuilder<StorageBackend> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(ResourcesDbProperties.DbTablePrefix + "StorageBackends", 
                           ResourcesDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.BackendType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // List columns
            builder.Property(x => x.AllowedMimeTypes)
                .HasConversion(
                    v => string.Join(';', v),
                    v => v.Split(';', System.StringSplitOptions.RemoveEmptyEntries).ToList()
                )
                .HasMaxLength(2000);

            // JSON columns
            builder.Property(x => x.Config)
                .HasColumnType("jsonb");

            // Indexes
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasIndex(x => x.IsDefault);
            builder.HasIndex(x => x.TenantId);
        }
    }
}
