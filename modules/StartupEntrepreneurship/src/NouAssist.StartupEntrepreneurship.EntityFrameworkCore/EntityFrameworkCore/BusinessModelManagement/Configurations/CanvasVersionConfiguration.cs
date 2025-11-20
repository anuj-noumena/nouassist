using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.EntityFrameworkCore.Configurations
{
    public class CanvasVersionConfiguration : IEntityTypeConfiguration<CanvasVersion>
    {
        public void Configure(EntityTypeBuilder<CanvasVersion> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(BusinessModelManagementDbProperties.DbTablePrefix + "CanvasVersions",
                           BusinessModelManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.CanvasId)
                .IsRequired();

            builder.Property(x => x.VersionName)
                .IsRequired()
                .HasMaxLength(BusinessModelManagementConsts.BusinessModelCanvas.MaxVersionNameLength);

            builder.Property(x => x.VersionNumber)
                .IsRequired();

            builder.Property(x => x.Notes)
                .HasMaxLength(BusinessModelManagementConsts.BusinessModelCanvas.MaxVersionNotesLength);

            builder.Property(x => x.IsCurrentVersion)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .IsRequired();

            // Indexes
            builder.HasIndex(x => x.CanvasId);
            builder.HasIndex(x => new { x.CanvasId, x.VersionNumber }).IsUnique();
            builder.HasIndex(x => new { x.CanvasId, x.IsCurrentVersion });
        }
    }
}
