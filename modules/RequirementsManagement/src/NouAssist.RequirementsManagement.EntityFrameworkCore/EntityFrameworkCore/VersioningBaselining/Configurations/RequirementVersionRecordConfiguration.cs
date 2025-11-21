using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.VersioningBaselining.Constants;
using NouAssist.RequirementsManagement.VersioningBaselining.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.VersioningBaselining.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for RequirementVersionRecord entity
    /// </summary>
    public class RequirementVersionRecordConfiguration : IEntityTypeConfiguration<RequirementVersionRecord>
    {
        public void Configure(EntityTypeBuilder<RequirementVersionRecord> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(
                VersionControlDbProperties.DbTablePrefix + "RequirementVersions",
                VersionControlDbProperties.DbSchema);

            // Combined key - RequirementId + Version
            builder.HasKey(x => new { x.RequirementId, x.Version.Value });

            // Properties
            builder.Property(x => x.RequirementId)
                .IsRequired();

            builder.Property(x => x.Version)
                .HasConversion(
                    v => v.Value,
                    v => new ValueObjects.VersionIdentifier(v))
                .HasMaxLength(VersionControlConsts.MaxVersionLength)
                .HasColumnName("Version")
                .IsRequired();

            // Complex value objects
            builder.OwnsOne(x => x.Author, authorBuilder =>
            {
                authorBuilder.Property(a => a.Id)
                    .HasColumnName("AuthorId")
                    .IsRequired();
                
                authorBuilder.Property(a => a.Name)
                    .HasColumnName("AuthorName")
                    .HasMaxLength(VersionControlConsts.MaxAuthorNameLength)
                    .IsRequired();
            });

            builder.OwnsOne(x => x.ChangeComment, commentBuilder =>
            {
                commentBuilder.Property(c => c.Text)
                    .HasColumnName("ChangeComment")
                    .HasMaxLength(VersionControlConsts.MaxChangeCommentLength)
                    .IsRequired();
            });

            builder.Property(x => x.CreationTime)
                .IsRequired();

            builder.Property(x => x.ContentSnapshot)
                .IsRequired();

            // Indexing
            builder.HasIndex(x => x.CreationTime);
        }
    }
}
