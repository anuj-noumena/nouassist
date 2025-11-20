using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryItem;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Constants;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using NouAssist.BusinessOperations.ResourceInventoryManagement.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for InventoryItemDefinition aggregate
    /// </summary>
    public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItemDefinitionAggregate>
    {
        public void Configure(EntityTypeBuilder<InventoryItemDefinitionAggregate> builder)
        {
            // Apply ABP base conventions (auditing, concurrency, etc.)
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(RIMDbProperties.DbTablePrefix + "InventoryItems", RIMDbProperties.DbSchema);

            // Primary key (handled by Entity<Guid> / AggregateRoot<Guid>)
            builder.HasKey(x => x.Id);

            // Properties and Value Object conversions
            builder.Property(x => x.Sku)
                .HasConversion(
                    v => v.Value,
                    v => new StockKeepingUnit(v))
                .IsRequired()
                .HasMaxLength(RIMConstants.InventoryItems.MaxSkuLength);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(RIMConstants.InventoryItems.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(RIMConstants.InventoryItems.MaxDescriptionLength);

            builder.Property(x => x.CategoryPath)
                .HasConversion(
                    v => v.Path,
                    v => new CategoryPath(v))
                .HasMaxLength(500);

            builder.Property(x => x.Barcode)
                .HasMaxLength(RIMConstants.InventoryItems.MaxBarcodeLength);

            builder.Property(x => x.UnitOfMeasure)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.MinimumStockLevel)
                .HasPrecision(18, 6)
                .HasDefaultValue(0);

            builder.Property(x => x.TargetStockLevel)
                .HasPrecision(18, 6)
                .HasDefaultValue(0);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.Tags)
                .HasMaxLength(500);

            // Relationships - one-to-many with StockLevel and InventoryTransaction
            builder.HasMany(x => x.StockLevels)
                .WithOne()
                .HasForeignKey(x => x.InventoryItemId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Transactions)
                .WithOne()
                .HasForeignKey(x => x.InventoryItemId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.Sku.Value).IsUnique();
            builder.HasIndex(x => x.Barcode);
            builder.HasIndex(x => x.CategoryPath.Path);
            builder.HasIndex(x => x.IsActive);
        }
    }

    /// <summary>
    /// Entity configuration for StockLevel entity
    /// </summary>
    public class StockLevelConfiguration : IEntityTypeConfiguration<StockLevel>
    {
        public void Configure(EntityTypeBuilder<StockLevel> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(RIMDbProperties.DbTablePrefix + "StockLevels", RIMDbProperties.DbSchema);

            // Primary key - composite key of InventoryItemId and LocationId
            builder.HasKey(x => new { x.InventoryItemId, x.LocationId });

            // Properties
            builder.Property(x => x.Quantity)
                .HasPrecision(18, 6)
                .HasDefaultValue(0);

            builder.Property(x => x.LastUpdated)
                .IsRequired();

            // Foreign key to StorageLocation
            builder.HasOne<Aggregates.StorageLocation.StorageLocationAggregate>()
                .WithMany()
                .HasForeignKey(x => x.LocationId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes (besides the primary key)
            builder.HasIndex(x => x.LocationId);
        }
    }

    /// <summary>
    /// Entity configuration for InventoryTransaction entity
    /// </summary>
    public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
    {
        public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(RIMDbProperties.DbTablePrefix + "InventoryTransactions", RIMDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.InventoryItemId)
                .IsRequired();

            builder.Property(x => x.LocationId)
                .IsRequired();

            builder.Property(x => x.TransactionType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.PreviousQuantity)
                .HasPrecision(18, 6)
                .IsRequired();

            builder.Property(x => x.NewQuantity)
                .HasPrecision(18, 6)
                .IsRequired();

            builder.Property(x => x.AdjustmentAmount)
                .HasPrecision(18, 6)
                .IsRequired();

            builder.Property(x => x.UnitOfMeasure)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.TransactionReference)
                .HasMaxLength(100);

            builder.Property(x => x.Notes)
                .HasMaxLength(500);

            builder.Property(x => x.Timestamp)
                .IsRequired();

            // Foreign key to StorageLocation
            builder.HasOne<Aggregates.StorageLocation.StorageLocationAggregate>()
                .WithMany()
                .HasForeignKey(x => x.LocationId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.InventoryItemId);
            builder.HasIndex(x => x.LocationId);
            builder.HasIndex(x => x.TransactionType);
            builder.HasIndex(x => x.Timestamp);
            builder.HasIndex(x => x.TransactionReference);
        }
    }
}
