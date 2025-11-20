using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceType;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Constants;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for ResourceType aggregate
    /// </summary>
    public class ResourceTypeConfiguration : IEntityTypeConfiguration<ResourceTypeAggregate>
    {
        public void Configure(EntityTypeBuilder<ResourceTypeAggregate> builder)
        {
            // Apply ABP base conventions (auditing, concurrency, etc.)
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(RIMDbProperties.DbTablePrefix + "ResourceTypes", RIMDbProperties.DbSchema);

            // Primary key (handled by Entity<Guid> / AggregateRoot<Guid>)
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(RIMConstants.ResourceTypes.MaxCodeLength);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(RIMConstants.ResourceTypes.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(RIMConstants.ResourceTypes.MaxDescriptionLength);

            builder.Property(x => x.Category)
                .IsRequired()
                .HasConversion<string>();

            // Value object conversions
            builder.Property(x => x.CategoryPath)
                .HasConversion(
                    v => v.Path,
                    v => new ValueObjects.CategoryPath(v))
                .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.Icon)
                .HasMaxLength(100);

            builder.Property(x => x.Color)
                .HasMaxLength(20);

            // Relationships - one-to-many with ResourceAttributeDefinition
            builder.HasMany(x => x.AttributeDefinitions)
                .WithOne()
                .HasForeignKey(x => x.ResourceTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Index on Code (for lookups)
            builder.HasIndex(x => x.Code).IsUnique();
            
            // Index on Category (for filtering)
            builder.HasIndex(x => x.Category);
            
            // Index on CategoryPath (for hierarchical queries)
            builder.HasIndex(x => x.CategoryPath.Path);
        }
    }

    /// <summary>
    /// Entity configuration for ResourceAttributeDefinition entity
    /// </summary>
    public class ResourceAttributeDefinitionConfiguration : IEntityTypeConfiguration<ResourceAttributeDefinition>
    {
        public void Configure(EntityTypeBuilder<ResourceAttributeDefinition> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(RIMDbProperties.DbTablePrefix + "ResourceAttributeDefinitions", RIMDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => new { x.ResourceTypeId, x.Name });

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(RIMConstants.Attributes.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(RIMConstants.Attributes.MaxDescriptionLength);

            builder.Property(x => x.DataType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.IsRequired)
                .IsRequired();

            builder.Property(x => x.DefaultValue)
                .HasMaxLength(RIMConstants.Attributes.MaxValueLength);

            builder.Property(x => x.PossibleValues)
                .HasMaxLength(1000);

            builder.Property(x => x.DisplayOrder)
                .HasDefaultValue(0);

            // Index on ResourceTypeId (for filtering)
            builder.HasIndex(x => x.ResourceTypeId);
        }
    }
}
