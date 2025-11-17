using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.ProjectManagement.Configurations
{
    public class ProjectSprintConfiguration : IEntityTypeConfiguration<ProjectSprint>
    {
        public void Configure(EntityTypeBuilder<ProjectSprint> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(WorkManagementDbProperties.DbTablePrefix + "ProjectSprints",
                WorkManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // === RELATIONSHIPS ===
            builder.Property(x => x.ProjectId)
                .IsRequired();

            builder.Property(x => x.TenantId)
                .IsRequired(false);

            // === BASIC INFO ===
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Goal)
                .IsRequired()
                .HasMaxLength(1000);

            // === TIMELINE (Value Object - Owned Entity) ===
            builder.OwnsOne(x => x.Timeline, timeline =>
            {
                timeline.Property(t => t.StartDate)
                    .HasColumnName("StartDate")
                    .IsRequired();

                timeline.Property(t => t.EndDate)
                    .HasColumnName("EndDate")
                    .IsRequired();

                timeline.Property(t => t.ActualStartDate)
                    .HasColumnName("ActualStartDate")
                    .IsRequired(false);

                timeline.Property(t => t.ActualEndDate)
                    .HasColumnName("ActualEndDate")
                    .IsRequired(false);
            });

            // === METRICS (Value Object - Owned Entity) ===
            builder.OwnsOne(x => x.Metrics, metrics =>
            {
                metrics.Property(m => m.PlannedStoryPoints)
                    .HasColumnName("PlannedStoryPoints")
                    .IsRequired()
                    .HasDefaultValue(0);

                metrics.Property(m => m.CompletedStoryPoints)
                    .HasColumnName("CompletedStoryPoints")
                    .IsRequired()
                    .HasDefaultValue(0);

                metrics.Property(m => m.VelocityEstimate)
                    .HasColumnName("VelocityEstimate")
                    .IsRequired()
                    .HasDefaultValue(0);

                metrics.Property(m => m.ActualVelocity)
                    .HasColumnName("ActualVelocity")
                    .IsRequired()
                    .HasDefaultValue(0);
            });

            // === STATUS ===
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.RetrospectiveNotes)
                .HasMaxLength(5000);

            builder.Property(x => x.ReviewNotes)
                .HasMaxLength(5000);

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(ProjectSprint.ExtraProperties));

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40);

            // === INDEXES ===
            builder.HasIndex(x => x.ProjectId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.TenantId);
            builder.HasIndex(x => new { x.ProjectId, x.Status });
        }
    }
}