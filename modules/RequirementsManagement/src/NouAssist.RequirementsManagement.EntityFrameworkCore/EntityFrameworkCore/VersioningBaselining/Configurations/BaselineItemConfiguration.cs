using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.VersioningBaselining.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.VersioningBaselining.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for BaselineItem entity
    /// </summary>
    public class BaselineItemConfiguration : IEntityTypeConfiguration<BaselineItem>
    {
        public void Configure(EntityTypeBuilder<BaselineItem> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(
                VersionControlDbProperties.DbTablePrefix + "BaselineItems",
                VersionControlDbProperties.DbSchema);

            // Combined key - BaselineId + RequirementId
            builder.HasKey(x => new { x.BaselineId, x.RequirementId });

            // Properties
            builder.Property(x => x.BaselineId)
                .IsRequired();

            builder.Property(x => x.RequirementId)
                .IsRequired();

            builder.Property(x => x.Version)
                .HasConversion(
                    v => v.Value,
                    v => new ValueObjects.VersionIdentifier(v))
                .HasMaxLength(VersionControlConsts.MaxVersionLength)
                .HasColumnName("Version")
                .IsRequired();

            builder.Property(x => x.AddedAt)
                .IsRequired();

            // Indexing
            builder.HasIndex(x => x.RequirementId);
        }
    }
}
