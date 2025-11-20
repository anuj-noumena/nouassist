using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.Specification.Entities;

namespace NouAssist.RequirementsManagement.Specification.EntityFrameworkCore.Configurations
{
    public class RequirementScopeMarkerConfiguration : IEntityTypeConfiguration<RequirementScopeMarker>
    {
        public void Configure(EntityTypeBuilder<RequirementScopeMarker> builder)
        {
            builder.ToTable(RequirementSpecificationManagementDbProperties.DbTablePrefix + "RequirementScopeMarkers",
                            RequirementSpecificationManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.RequirementId)
                .IsRequired();

            builder.Property(x => x.ScopeStatus)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Justification)
                .HasMaxLength(1000);

            builder.Property(x => x.MarkedAt)
                .IsRequired();

            builder.Property(x => x.MarkedBy)
                .IsRequired();

            // Configure indexes
            builder.HasIndex(x => x.RequirementId)
                   .IsUnique()
                   .HasDatabaseName("IX_RequirementScopeMarkers_RequirementId");

            builder.HasIndex(x => x.ScopeStatus)
                   .HasDatabaseName("IX_RequirementScopeMarkers_ScopeStatus");
        }
    }
}
