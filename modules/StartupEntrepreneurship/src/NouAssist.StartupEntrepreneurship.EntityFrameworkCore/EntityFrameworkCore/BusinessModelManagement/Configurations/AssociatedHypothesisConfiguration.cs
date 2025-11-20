using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Entities;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.EntityFrameworkCore.Configurations
{
    public class AssociatedHypothesisConfiguration : IEntityTypeConfiguration<AssociatedHypothesis>
    {
        public void Configure(EntityTypeBuilder<AssociatedHypothesis> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(BusinessModelManagementDbProperties.DbTablePrefix + "AssociatedHypotheses",
                           BusinessModelManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.CanvasId)
                .IsRequired();

            builder.Property(x => x.ComponentId)
                .IsRequired();

            builder.Property(x => x.ComponentType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .IsRequired();

            // Value Object Configuration - HypothesisDetails
            builder.OwnsOne(x => x.Details, detailsBuilder =>
            {
                detailsBuilder.Property(d => d.Assumption)
                    .HasColumnName("Assumption")
                    .IsRequired()
                    .HasMaxLength(BusinessModelManagementConsts.Hypothesis.MaxAssumptionLength);

                detailsBuilder.Property(d => d.TestDescription)
                    .HasColumnName("TestDescription")
                    .HasMaxLength(BusinessModelManagementConsts.Hypothesis.MaxTestDescriptionLength);

                detailsBuilder.Property(d => d.TestResult)
                    .HasColumnName("TestResult")
                    .HasMaxLength(BusinessModelManagementConsts.Hypothesis.MaxResultDescriptionLength);

                detailsBuilder.Property(d => d.Status)
                    .HasColumnName("Status")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                detailsBuilder.Property(d => d.TestStartDate)
                    .HasColumnName("TestStartDate");

                detailsBuilder.Property(d => d.TestEndDate)
                    .HasColumnName("TestEndDate");
            });

            // Indexes
            builder.HasIndex(x => x.CanvasId);
            builder.HasIndex(x => x.ComponentId);
            builder.HasIndex(x => new { x.CanvasId, x.ComponentType });
        }
    }
}
