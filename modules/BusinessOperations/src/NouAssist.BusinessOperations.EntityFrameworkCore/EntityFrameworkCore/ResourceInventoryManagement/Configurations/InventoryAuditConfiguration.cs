using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryAudit;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for InventoryAuditSession aggregate
    /// </summary>
    public class InventoryAuditSessionConfiguration : IEntityTypeConfiguration<InventoryAuditSessionAggregate>
    {
        public void Configure(EntityTypeBuilder<InventoryAuditSessionAggregate> builder)
        {
            // Apply ABP base conventions (auditing, concurrency, etc.)
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(RIMDbProperties.DbTablePrefix + "InventoryAuditSessions", RIMDbProperties.DbSchema);

            // Primary key (handled by Entity<Guid> / AggregateRoot<Guid>)
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.LocationId)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.ScheduledStartTime)
                .IsRequired();

            builder.Property(x => x.ActualStartTime);

            builder.Property(x => x.CompletionTime);

            builder.Property(x => x.AuditedByUserId)
                .IsRequired();

            builder.Property(x => x.Notes)
                .HasMaxLength(1000);

            // Relationships - one-to-many with AuditDiscrepancy
            builder.HasMany(x => x.Discrepancies)
                .WithOne()
                .HasForeignKey(x => x.AuditSessionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Foreign key to StorageLocation
            builder.HasOne<Aggregates.StorageLocation.StorageLocationAggregate>()
                .WithMany()
                .HasForeignKey(x => x.LocationId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.LocationId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.ScheduledStartTime);
            builder.HasIndex(x => x.AuditedByUserId);
        }
    }

    /// <summary>
    /// Entity configuration for AuditDiscrepancy entity
    /// </summary>
    public class AuditDiscrepancyConfiguration : IEntityTypeConfiguration<AuditDiscrepancy>
    {
        public void Configure(EntityTypeBuilder<AuditDiscrepancy> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(RIMDbProperties.DbTablePrefix + "AuditDiscrepancies", RIMDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.AuditSessionId)
                .IsRequired();

            builder.Property(x => x.InventoryItemId)
                .IsRequired();

            builder.Property(x => x.ExpectedQuantity)
                .HasPrecision(18, 6)
                .IsRequired();

            builder.Property(x => x.ActualQuantity)
                .HasPrecision(18, 6)
                .IsRequired();

            builder.Property(x => x.Reason)
                .HasMaxLength(500);

            builder.Property(x => x.Notes)
                .HasMaxLength(500);

            builder.Property(x => x.RecordedAt)
                .IsRequired();

            builder.Property(x => x.IsResolved)
                .IsRequired();

            builder.Property(x => x.ResolutionNotes)
                .HasMaxLength(500);

            builder.Property(x => x.ResolvedAt);

            // Foreign key to InventoryItem
            builder.HasOne<Aggregates.InventoryItem.InventoryItemDefinitionAggregate>()
                .WithMany()
                .HasForeignKey(x => x.InventoryItemId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.AuditSessionId);
            builder.HasIndex(x => x.InventoryItemId);
            builder.HasIndex(x => x.IsResolved);
            builder.HasIndex(x => x.RecordedAt);
        }
    }
}
