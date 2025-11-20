using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.ChangeManagement.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.ChangeManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity type configuration for ChangeImpactStatement entity
    /// </summary>
    public class ChangeImpactStatementConfiguration : IEntityTypeConfiguration<ChangeImpactStatement>
    {
        public void Configure(EntityTypeBuilder<ChangeImpactStatement> builder)
        {
            // Apply ABP base conventions for audited entity
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(
                ChangeManagementDbProperties.DbTablePrefix + "ChangeImpactStatements",
                ChangeManagementDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.ChangeRequestId)
                .IsRequired();

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(ChangeManagementConsts.MaxDescriptionLength);

            builder.Property(x => x.AssessmentDate)
                .IsRequired();

            // Value object mapping for ImpactDetails
            builder.OwnsOne(x => x.ImpactDetails, detailsBuilder =>
            {
                detailsBuilder.Property(d => d.Type)
                    .HasColumnName("ImpactType")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                detailsBuilder.Property(d => d.CostImpact)
                    .HasColumnName("CostImpact")
                    .HasColumnType("decimal(18,2)");

                detailsBuilder.Property(d => d.TimeImpactDays)
                    .HasColumnName("TimeImpactDays");

                detailsBuilder.Property(d => d.Dependencies)
                    .HasColumnName("Dependencies")
                    .HasMaxLength(1000);

                detailsBuilder.WithOwner();
            });

            // Indexes
            builder.HasIndex(x => x.ChangeRequestId);
            builder.HasIndex("ImpactType");
        }
    }
}
