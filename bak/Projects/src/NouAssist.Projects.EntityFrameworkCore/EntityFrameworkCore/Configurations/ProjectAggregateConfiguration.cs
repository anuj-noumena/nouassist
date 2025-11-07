using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Projects.Aggregates;
using NouAssist.Projects.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.EntityFrameworkCore.Projects.Configurations
{
    public class ProjectAggregateConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            // Apply ABP conventions (auditing, soft delete, multi-tenancy, concurrency)
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable("Projects", "Projects");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Identity & Relationships
            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.ParentProjectId)
                .IsRequired(false);

            builder.Property(x => x.GoalId)
                .IsRequired(false);

            builder.Property(x => x.WorkflowSchemeId)
                .IsRequired(false);

            builder.Property(x => x.ContextId)
                .IsRequired(false);

            builder.Property(x => x.AreaId)
                .IsRequired(false);

            builder.Property(x => x.TenantId)
                .IsRequired(false);

            // Basic Info
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(ProjectDomainConstants.MaxTitleLength);

            builder.Property(x => x.Description)
                .HasMaxLength(ProjectDomainConstants.MaxDescriptionLength);

            builder.Property(x => x.Summary)
                .HasMaxLength(ProjectDomainConstants.MaxSummaryLength);

            builder.Property(x => x.ExpectedOutcome)
                .HasMaxLength(ProjectDomainConstants.MaxDescriptionLength);

            // Status & Health
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.IsArchived)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.ArchivedAt)
                .IsRequired(false);

            // Priority & Classification
            builder.Property(x => x.Importance)
                .IsRequired()
                .HasDefaultValue(0.5);

            builder.Property(x => x.Urgency)
                .IsRequired()
                .HasDefaultValue(0.5);

            builder.Property(x => x.EisenhowerQuadrant)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Dates & Progress
            builder.Property(x => x.StartDate)
                .IsRequired(false);

            builder.Property(x => x.DueDate)
                .IsRequired(false);

            builder.Property(x => x.CompletedAt)
                .IsRequired(false);

            builder.Property(x => x.Progress)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.LastReviewedAt)
                .IsRequired(false);

            // Team Management
            builder.Property(x => x.PrimaryTeamId)
                .IsRequired(false);

            // Approval
            builder.Property(x => x.RequiresApproval)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.ApprovalWorkflowId)
                .IsRequired(false);

            builder.Property(x => x.CurrentApprovalRequestId)
                .IsRequired(false);

            // Extra Properties (handled by ABP)
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(Project.ExtraProperties));

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40);

            // Indexes
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.DueDate);
            builder.HasIndex(x => x.TenantId);
            builder.HasIndex(x => new { x.Status, x.IsArchived });
            builder.HasIndex(x => new { x.OwnerId, x.Status });
        }
    }
}
