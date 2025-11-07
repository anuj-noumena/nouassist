using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.CollaborationHub.Resources.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.CollaborationHub.Resources.EntityFrameworkCore.Configurations
{
    public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
    {
        public void Configure(EntityTypeBuilder<InventoryItem> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(ResourcesDbProperties.DbTablePrefix + "InventoryItems", 
                           ResourcesDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId).IsRequired();

            builder.Property(x => x.Kind)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.Manufacturer)
                .HasMaxLength(256);

            builder.Property(x => x.Model)
                .HasMaxLength(256);

            builder.Property(x => x.Barcode)
                .HasMaxLength(100);

            builder.Property(x => x.SerialNumber)
                .HasMaxLength(100);

            builder.Property(x => x.Sku)
                .HasMaxLength(100);

            builder.Property(x => x.Isbn)
                .HasMaxLength(20);

            builder.Property(x => x.Condition)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Location)
                .HasMaxLength(256);

            builder.Property(x => x.Building)
                .HasMaxLength(256);

            builder.Property(x => x.Zone)
                .HasMaxLength(256);

            builder.Property(x => x.ImageUrl)
                .HasMaxLength(2048);

            builder.Property(x => x.MaintenanceNotes)
                .HasMaxLength(4000);

            builder.Property(x => x.PurchasePrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.CurrentValue)
                .HasColumnType("decimal(18,2)");

            // List columns
            builder.Property(x => x.AdditionalImageUrls)
                .HasConversion(
                    v => string.Join(';', v),
                    v => v.Split(';', System.StringSplitOptions.RemoveEmptyEntries).ToList()
                )
                .HasMaxLength(4000);

            builder.Property(x => x.Tags)
                .HasConversion(
                    v => string.Join(';', v),
                    v => v.Split(';', System.StringSplitOptions.RemoveEmptyEntries).ToList()
                )
                .HasMaxLength(1000);

            // JSON columns
            builder.Property(x => x.CustomFields)
                .HasColumnType("jsonb");

            // Indexes
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.Kind);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.Barcode).IsUnique();
            builder.HasIndex(x => x.SerialNumber).IsUnique();
            builder.HasIndex(x => x.Location);
            builder.HasIndex(x => x.AssignedToUserId);
            builder.HasIndex(x => x.DueBackAt);
            builder.HasIndex(x => x.TenantId);
        }
    }
}
