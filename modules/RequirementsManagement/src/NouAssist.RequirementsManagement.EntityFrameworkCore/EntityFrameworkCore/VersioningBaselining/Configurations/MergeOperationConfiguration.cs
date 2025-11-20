using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.VersioningBaselining.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.VersioningBaselining.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for MergeOperation entity
    /// </summary>
    public class MergeOperationConfiguration : IEntityTypeConfiguration<MergeOperation>
    {
        public void Configure(EntityTypeBuilder<MergeOperation> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(
                VersionControlDbProperties.DbTablePrefix + "MergeOperations",
                VersionControlDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.SourceBranchId)
                .IsRequired();

            builder.Property(x => x.TargetBranchId)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            // Complex value objects
            builder.OwnsOne(x => x.Result, resultBuilder =>
            {
                resultBuilder.Property(r => r.Status)
                    .HasColumnName("ResultStatus")
                    .HasConversion<string>()
                    .HasMaxLength(50);
                
                resultBuilder.Property(r => r.MergedRequirementCount)
                    .HasColumnName("MergedRequirementCount");
                
                resultBuilder.Property(r => r.ConflictCount)
                    .HasColumnName("ConflictCount");
                
                resultBuilder.Property(r => r.Message)
                    .HasColumnName("ResultMessage")
                    .HasMaxLength(2000);
            });

            builder.OwnsOne(x => x.InitiatedBy, authorBuilder =>
            {
                authorBuilder.Property(a => a.Id)
                    .HasColumnName("InitiatedById")
                    .IsRequired();
                
                authorBuilder.Property(a => a.Name)
                    .HasColumnName("InitiatedByName")
                    .HasMaxLength(VersionControlConsts.MaxAuthorNameLength)
                    .IsRequired();
            });

            builder.Property(x => x.InitiatedAt)
                .IsRequired();

            builder.OwnsOne(x => x.CompletedBy, authorBuilder =>
            {
                authorBuilder.Property(a => a.Id)
                    .HasColumnName("CompletedById");
                
                authorBuilder.Property(a => a.Name)
                    .HasColumnName("CompletedByName")
                    .HasMaxLength(VersionControlConsts.MaxAuthorNameLength);
            });

            builder.Property(x => x.CompletedAt);

            // Indexing
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.SourceBranchId);
            builder.HasIndex(x => x.TargetBranchId);
        }
    }
}
