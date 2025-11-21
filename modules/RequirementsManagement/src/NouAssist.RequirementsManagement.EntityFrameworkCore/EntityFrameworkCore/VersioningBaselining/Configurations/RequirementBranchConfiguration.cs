using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.VersioningBaselining.Aggregates;
using NouAssist.RequirementsManagement.VersioningBaselining.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.VersioningBaselining.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for RequirementBranch aggregate
    /// </summary>
    public class RequirementBranchConfiguration : IEntityTypeConfiguration<RequirementBranch>
    {
        public void Configure(EntityTypeBuilder<RequirementBranch> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(
                VersionControlDbProperties.DbTablePrefix + "Branches",
                VersionControlDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .HasConversion(
                    v => v.Value,
                    v => new ValueObjects.BranchName(v))
                .HasMaxLength(VersionControlConsts.MaxBranchNameLength)
                .HasColumnName("Name")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.SourceId)
                .IsRequired();

            builder.Property(x => x.SourceType)
                .HasMaxLength(50)
                .IsRequired();

            // Complex value objects
            builder.OwnsOne(x => x.CreatedBy, authorBuilder =>
            {
                authorBuilder.Property(a => a.Id)
                    .HasColumnName("CreatedById")
                    .IsRequired();
                
                authorBuilder.Property(a => a.Name)
                    .HasColumnName("CreatedByName")
                    .HasMaxLength(VersionControlConsts.MaxAuthorNameLength)
                    .IsRequired();
            });

            // Relationships
            builder.HasMany(x => x.MergeOperations)
                .WithOne()
                .HasForeignKey("BranchId")
                .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(b => b.Name).IsUnique();
            builder.HasIndex(b => b.Status);
            builder.HasIndex(b => new { b.SourceId, b.SourceType });
        }
    }
}
