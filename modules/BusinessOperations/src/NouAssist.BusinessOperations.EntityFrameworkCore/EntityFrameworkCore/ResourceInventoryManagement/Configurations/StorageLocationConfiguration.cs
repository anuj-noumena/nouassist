using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.StorageLocation;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Constants;
using NouAssist.BusinessOperations.ResourceInventoryManagement.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for StorageLocation aggregate
    /// </summary>
    public class StorageLocationConfiguration : IEntityTypeConfiguration<StorageLocationAggregate>
    {
        public void Configure(EntityTypeBuilder<StorageLocationAggregate> builder)
        {
            // Apply ABP base conventions (auditing, concurrency, etc.)
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(RIMDbProperties.DbTablePrefix + "StorageLocations", RIMDbProperties.DbSchema);

            // Primary key (handled by Entity<Guid> / AggregateRoot<Guid>)
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(RIMConstants.StorageLocations.MaxCodeLength);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(RIMConstants.StorageLocations.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(RIMConstants.StorageLocations.MaxDescriptionLength);

            builder.Property(x => x.LocationType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Address)
                .HasMaxLength(500);

            // Value Object conversions
            builder.OwnsOne(x => x.Coordinates, coordinatesBuilder =>
            {
                coordinatesBuilder.Property(c => c.Latitude)
                    .HasColumnName("Latitude")
                    .HasPrecision(9, 6);

                coordinatesBuilder.Property(c => c.Longitude)
                    .HasColumnName("Longitude")
                    .HasPrecision(9, 6);
            });

            builder.Property(x => x.MaxCapacity)
                .HasPrecision(18, 6);

            builder.Property(x => x.CapacityUnitOfMeasure)
                .HasMaxLength(50);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.Tags)
                .HasMaxLength(500);

            // Self-referencing relationship for hierarchy
            builder.HasOne<StorageLocationAggregate>()
                .WithMany()
                .HasForeignKey(x => x.ParentLocationId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-many relationship with child locations
            builder.HasMany(x => x.ChildLocations)
                .WithOne()
                .HasForeignKey(x => x.ParentLocationId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.Code).IsUnique();
            builder.HasIndex(x => x.ParentLocationId);
            builder.HasIndex(x => x.LocationType);
            builder.HasIndex(x => x.IsActive);
        }
    }

    /// <summary>
    /// Entity configuration for LocationHierarchyNode entity
    /// </summary>
    public class LocationHierarchyNodeConfiguration : IEntityTypeConfiguration<LocationHierarchyNode>
    {
        public void Configure(EntityTypeBuilder<LocationHierarchyNode> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(RIMDbProperties.DbTablePrefix + "LocationHierarchyNodes", RIMDbProperties.DbSchema);

            // Primary key - composite key of ParentLocationId and ChildLocationId
            builder.HasKey(x => new { x.ParentLocationId, x.ChildLocationId });

            // Properties
            builder.Property(x => x.ChildLocationCode)
                .IsRequired()
                .HasMaxLength(RIMConstants.StorageLocations.MaxCodeLength);

            builder.Property(x => x.ChildLocationName)
                .IsRequired()
                .HasMaxLength(RIMConstants.StorageLocations.MaxNameLength);

            builder.Property(x => x.DisplayOrder)
                .HasDefaultValue(0);

            // Indexes (besides the primary key)
            builder.HasIndex(x => x.ChildLocationId);
        }
    }
}
