using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.ProjectManagement.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.ProjectManagement.Configurations
{
    public class ProjectEventConfiguration : IEntityTypeConfiguration<ProjectEvent>
    {
        public void Configure(EntityTypeBuilder<ProjectEvent> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(WorkManagementDbProperties.DbTablePrefix + "ProjectEvents",
                WorkManagementDbProperties.DbSchema);

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

            builder.Property(x => x.EventType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Event Details
            builder.Property(x => x.EventDate)
                .IsRequired();

            builder.Property(x => x.EndDate)
                .IsRequired(false);

            builder.Property(x => x.IsAllDay)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.Location)
                .HasMaxLength(500);

            // Related Entities
            builder.Property(x => x.RelatedSprintId)
                .IsRequired(false);

            builder.Property(x => x.RelatedMilestoneId)
                .IsRequired(false);

            // Completion
            builder.Property(x => x.IsCompleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.CompletedAt)
                .IsRequired(false);

            // // Extra Properties
            // builder.Property(x => x.ExtraProperties)
            //     .HasColumnName(nameof(ProjectEvent.ExtraProperties));
            //
            // // Concurrency Stamp
            // builder.Property(x => x.ConcurrencyStamp)
            //     .IsConcurrencyToken()
            //     .HasMaxLength(40);

            // Indexes
            builder.HasIndex(x => x.ProjectId);
            builder.HasIndex(x => x.EventDate);
            builder.HasIndex(x => x.EventType);
            builder.HasIndex(x => new { x.ProjectId, x.EventDate });
            builder.HasIndex(x => new { x.IsCompleted, x.EventDate });
        }
    }
}