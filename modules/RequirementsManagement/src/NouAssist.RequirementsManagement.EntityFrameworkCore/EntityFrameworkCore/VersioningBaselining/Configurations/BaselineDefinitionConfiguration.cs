using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.VersioningBaselining.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.VersioningBaselining.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for BaselineDefinition aggregate
    /// </summary>
    public class BaselineDefinitionConfiguration : IEntityTypeConfiguration<BaselineDefinition>
    {
        public void Configure(EntityTypeBuilder<BaselineDefinition> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(
                VersionControlDbProperties.DbTablePrefix + "Baselines",
                VersionControlDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .HasMaxLength(VersionControlConsts.MaxBaselineNameLength)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(VersionControlConsts.MaxBaselineDescriptionLength);

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            // Relationships
            builder.HasMany(x => x.Items)
                .WithOne()
                .HasForeignKey(i => i.BaselineId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.Status);
        }
    }
}
