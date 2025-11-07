using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Projects.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.EntityFrameworkCore.Projects.Configurations
{
    public class ProjectSprintConfiguration : IEntityTypeConfiguration<ProjectSprint>
    {
        public void Configure(EntityTypeBuilder<ProjectSprint> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable("ProjectSprints", "Projects");

            builder.HasKey(x => x.Id);

            // Relationships
            builder.Property(x => x.ProjectId)
                .IsRequired();

            builder.Property(x => x.TenantId)
                .IsRequired(false);

            // Basic Info
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Goal)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.ReleaseTheme)
                .HasMaxLength(500);

            // Dates
            builder.Property(x => x.StartDate)
                .IsRequired();

            builder.Property(x => x.EndDate)
                .IsRequired();

            // Metrics
            builder.Property(x => x.PlannedStoryPoints)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.CompletedStoryPoints)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.VelocityEstimate)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.ActualVelocity)
                .IsRequired()
                .HasDefaultValue(0);

            // Status
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.CompletedAt)
                .IsRequired(false);

            builder.Property(x => x.CompletedBy)
                .HasMaxLength(256);

            builder.Property(x => x.RetrospectiveNotes)
                .HasMaxLength(5000);

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(ProjectSprint.ExtraProperties));

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40);

            // Indexes
            builder.HasIndex(x => x.ProjectId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => new { x.StartDate, x.EndDate });
            builder.HasIndex(x => x.TenantId);
        }
    }
}
