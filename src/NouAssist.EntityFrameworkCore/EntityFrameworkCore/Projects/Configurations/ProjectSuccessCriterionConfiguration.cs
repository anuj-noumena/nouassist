using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Projects.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.EntityFrameworkCore.Projects.Configurations
{
    public class ProjectSuccessCriterionConfiguration : IEntityTypeConfiguration<ProjectSuccessCriterion>
    {
        public void Configure(EntityTypeBuilder<ProjectSuccessCriterion> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable("ProjectSuccessCriteria", "Projects");

            builder.HasKey(x => x.Id);

            // Relationships
            builder.Property(x => x.ProjectId)
                .IsRequired();

            // Basic Info
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.CriterionType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Measurement
            builder.Property(x => x.MeasurementMethod)
                .HasMaxLength(500);

            builder.Property(x => x.TargetValue)
                .HasMaxLength(500);

            builder.Property(x => x.ActualValue)
                .HasMaxLength(500);

            builder.Property(x => x.Unit)
                .HasMaxLength(50);

            builder.Property(x => x.TargetNumeric)
                .HasPrecision(18, 2);

            builder.Property(x => x.ActualNumeric)
                .HasPrecision(18, 2);

            // Status
            builder.Property(x => x.Weight)
                .IsRequired()
                .HasDefaultValue(1.0)
                .HasPrecision(5, 2);

            builder.Property(x => x.OrderIndex)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.IsMet)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.TargetDate)
                .IsRequired(false);

            builder.Property(x => x.MetAt)
                .IsRequired(false);

            builder.Property(x => x.MetBy)
                .HasMaxLength(256);

            // Documentation
            builder.Property(x => x.Notes)
                .HasMaxLength(5000);

            builder.Property(x => x.EvidenceUrl)
                .HasMaxLength(2000);

            // // Extra Properties
            // builder.Property(x => x.ExtraProperties)
            //     .HasColumnName(nameof(ProjectSuccessCriterion.ExtraProperties));
            //
            // // Concurrency Stamp
            // builder.Property(x => x.ConcurrencyStamp)
            //     .IsConcurrencyToken()
            //     .HasMaxLength(40);

            // Indexes
            builder.HasIndex(x => x.ProjectId);
            builder.HasIndex(x => new { x.ProjectId, x.OrderIndex });
            builder.HasIndex(x => new { x.ProjectId, x.IsMet });
        }
    }
}
