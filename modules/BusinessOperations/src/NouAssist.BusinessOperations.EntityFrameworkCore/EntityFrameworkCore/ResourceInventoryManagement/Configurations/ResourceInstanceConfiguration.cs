using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceInstance;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Constants;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using NouAssist.BusinessOperations.ResourceInventoryManagement.ValueObjects;
using System.Text.Json;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for ResourceInstance aggregate
    /// </summary>
    public class ResourceInstanceConfiguration : IEntityTypeConfiguration<ResourceInstanceAggregate>
    {
        public void Configure(EntityTypeBuilder<ResourceInstanceAggregate> builder)
        {
            // Apply ABP base conventions (auditing, concurrency, etc.)
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(RIMDbProperties.DbTablePrefix + "ResourceInstances", RIMDbProperties.DbSchema);

            // Primary key (handled by Entity<Guid> / AggregateRoot<Guid>)
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.ResourceTypeId)
                .IsRequired();

            builder.Property(x => x.Identifier)
                .IsRequired()
                .HasMaxLength(RIMConstants.ResourceInstances.MaxIdentifierLength);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(RIMConstants.ResourceInstances.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(RIMConstants.ResourceInstances.MaxDescriptionLength);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>();

            // Value Objects
            builder.Property(x => x.UserId)
                .HasConversion(
                    v => v != null ? v.Value : (Guid?)null,
                    v => v.HasValue ? new UserId(v.Value) : null);

            builder.Property(x => x.Tags)
                .HasMaxLength(500);

            // Store AttributeValues as JSON
            builder.Property<List<AttributeValue>>("_attributeValues")
                .HasColumnName("AttributeValues")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, JsonSerializerDefaultOptions.Options),
                    v => JsonSerializer.Deserialize<List<AttributeValue>>(v, JsonSerializerDefaultOptions.Options) ?? new List<AttributeValue>());

            // Relationships - one-to-many with AllocationRecord and UsageRecord
            builder.HasMany(x => x.AllocationRecords)
                .WithOne()
                .HasForeignKey(x => x.ResourceInstanceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.UsageRecords)
                .WithOne()
                .HasForeignKey(x => x.ResourceInstanceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Foreign key to ResourceType
            builder.HasOne<Aggregates.ResourceType.ResourceTypeAggregate>()
                .WithMany()
                .HasForeignKey(x => x.ResourceTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.Identifier).IsUnique();
            builder.HasIndex(x => x.ResourceTypeId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.UserId.Value);
        }
    }

    /// <summary>
    /// Entity configuration for AllocationRecord entity
    /// </summary>
    public class AllocationRecordConfiguration : IEntityTypeConfiguration<AllocationRecord>
    {
        public void Configure(EntityTypeBuilder<AllocationRecord> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(RIMDbProperties.DbTablePrefix + "AllocationRecords", RIMDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.ResourceInstanceId)
                .IsRequired();

            builder.Property(x => x.AllocatedToEntity)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.AllocatedToEntityId)
                .IsRequired();

            builder.Property(x => x.StartTime)
                .IsRequired();

            builder.Property(x => x.EndTime);

            builder.Property(x => x.AllocationPercentage)
                .HasPrecision(5, 2);

            builder.Property(x => x.Notes)
                .HasMaxLength(500);

            builder.Property(x => x.EndNotes)
                .HasMaxLength(500);

            // Indexes
            builder.HasIndex(x => x.ResourceInstanceId);
            builder.HasIndex(x => new { x.AllocatedToEntity, x.AllocatedToEntityId });
            builder.HasIndex(x => x.StartTime);
            builder.HasIndex(x => x.EndTime);
        }
    }

    /// <summary>
    /// Entity configuration for UsageRecord entity
    /// </summary>
    public class UsageRecordConfiguration : IEntityTypeConfiguration<UsageRecord>
    {
        public void Configure(EntityTypeBuilder<UsageRecord> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(RIMDbProperties.DbTablePrefix + "UsageRecords", RIMDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.ResourceInstanceId)
                .IsRequired();

            builder.Property(x => x.UsageType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.StartTime)
                .IsRequired();

            builder.Property(x => x.EndTime)
                .IsRequired();

            builder.Property(x => x.Quantity)
                .HasPrecision(18, 6);

            builder.Property(x => x.UnitOfMeasure)
                .HasMaxLength(50);

            builder.Property(x => x.RelatedEntityType)
                .HasMaxLength(100);

            builder.Property(x => x.Notes)
                .HasMaxLength(500);

            // Indexes
            builder.HasIndex(x => x.ResourceInstanceId);
            builder.HasIndex(x => x.UsageType);
            builder.HasIndex(x => x.StartTime);
            builder.HasIndex(x => x.EndTime);
            builder.HasIndex(x => new { x.RelatedEntityType, x.RelatedEntityId });
        }
    }
}
