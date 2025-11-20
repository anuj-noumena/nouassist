using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.VersioningBaselining.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.VersioningBaselining.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for VersionedRequirementHistory aggregate
    /// </summary>
    public class VersionedRequirementHistoryConfiguration : IEntityTypeConfiguration<VersionedRequirementHistory>
    {
        public void Configure(EntityTypeBuilder<VersionedRequirementHistory> builder)
        {
            // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency)
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(
                VersionControlDbProperties.DbTablePrefix + "RequirementHistories",
                VersionControlDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.RequirementId)
                .IsRequired();

            builder.Property(x => x.LatestVersion)
                .HasConversion(
                    v => v.Value,
                    v => new ValueObjects.VersionIdentifier(v))
                .HasMaxLength(VersionControlConsts.MaxVersionLength)
                .IsRequired();

            // Relationships
            builder.HasMany(x => x.VersionRecords)
                .WithOne()
                .HasForeignKey(v => v.RequirementId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.RequirementId).IsUnique();
        }
    }
}
