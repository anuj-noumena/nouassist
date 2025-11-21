using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.ChangeManagement.Constants;
using NouAssist.RequirementsManagement.ChangeManagement.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.ChangeManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity type configuration for ChangeRationale entity
    /// </summary>
    public class ChangeRationaleConfiguration : IEntityTypeConfiguration<ChangeRationale>
    {
        public void Configure(EntityTypeBuilder<ChangeRationale> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(
                ChangeManagementDbProperties.DbTablePrefix + "ChangeRationales",
                ChangeManagementDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.ChangeRequestId)
                .IsRequired();

            builder.Property(x => x.RationaleText)
                .IsRequired()
                .HasMaxLength(ChangeManagementConsts.MaxRationaleLength);

            builder.Property(x => x.Category)
                .HasMaxLength(100);

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            // Indexes
            builder.HasIndex(x => x.ChangeRequestId);
            builder.HasIndex(x => x.Category);
        }
    }
}
