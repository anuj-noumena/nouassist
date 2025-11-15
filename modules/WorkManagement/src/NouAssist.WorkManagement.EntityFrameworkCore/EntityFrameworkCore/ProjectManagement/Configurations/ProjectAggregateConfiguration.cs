using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using NouAssist.WorkManagement.ProjectManagement.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.ProjectManagement.Configurations
{
    public class ProjectAggregateConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable("Projects", "Projects");

            builder.HasKey(x => x.Id);

            // === IDENTITY & RELATIONSHIPS ===
            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.TenantId)
                .IsRequired(false);

            builder.Property(x => x.ParentProjectId)
                .IsRequired(false);

            builder.Property(x => x.AreaId)
                .IsRequired(false);

            builder.Property(x => x.GoalId)
                .IsRequired(false);

            builder.Property(x => x.WorkflowSchemeId)
                .IsRequired(false);

            // === CONTENT ===
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(ProjectDomainConstants.MaxTitleLength);

            builder.Property(x => x.Description)
                .HasMaxLength(ProjectDomainConstants.MaxDescriptionLength);

            builder.Property(x => x.Vision)
                .HasMaxLength(ProjectDomainConstants.MaxDescriptionLength);

            builder.Property(x => x.Purpose)
                .HasMaxLength(ProjectDomainConstants.MaxDescriptionLength);

            builder.Property(x => x.ExpectedOutcome)
                .HasMaxLength(ProjectDomainConstants.MaxDescriptionLength);

            // === WORKFLOW ===
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.IsArchived)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.ArchivedAt)
                .IsRequired(false);

            // === PRIORITY (Value Object - Owned Entity) ===
            builder.OwnsOne(x => x.Priority, priority =>
            {
                priority.Property(p => p.Importance)
                    .HasColumnName("Importance")
                    .IsRequired()
                    .HasDefaultValue(0.5);

                priority.Property(p => p.Urgency)
                    .HasColumnName("Urgency")
                    .IsRequired()
                    .HasDefaultValue(0.5);
            });

            // === TIMELINE (Value Object - Owned Entity) ===
            builder.OwnsOne(x => x.Timeline, timeline =>
            {
                timeline.Property(t => t.PlannedStartDate)
                    .HasColumnName("PlannedStartDate")
                    .IsRequired(false);

                timeline.Property(t => t.PlannedDueDate)
                    .HasColumnName("PlannedDueDate")
                    .IsRequired(false);

                timeline.Property(t => t.ActualStartDate)
                    .HasColumnName("ActualStartDate")
                    .IsRequired(false);

                timeline.Property(t => t.ActualCompletedDate)
                    .HasColumnName("ActualCompletedDate")
                    .IsRequired(false);
            });

            // === PROGRESS ===
            builder.Property(x => x.Progress)
                .IsRequired()
                .HasDefaultValue(0)
                .HasPrecision(5, 2);

            builder.Property(x => x.LastReviewedAt)
                .IsRequired(false);

            // === TEAM ===
            builder.Property(x => x.PrimaryTeamId)
                .IsRequired(false);

            // CollaboratingTeamIds - stored as JSON
            builder.Property("_collaboratingTeamIds")
                .HasColumnName("CollaboratingTeamIds")
                .HasColumnType("jsonb");

            // === APPROVAL ===
            builder.Property(x => x.ApprovalRequestId)
                .IsRequired(false);

            // === HIERARCHY ===
            builder.Property("_level")
                .HasColumnName("HierarchyLevel")
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property("_hierarchyPath")
                .HasColumnName("HierarchyPath")
                .HasMaxLength(4000);

            // === SUCCESS CRITERIA (Owned Collection) ===
            builder.OwnsMany(x => x.SuccessCriteria, successCriteria =>
            {
                successCriteria.ToTable("ProjectSuccessCriteria", "Projects");
                
                successCriteria.WithOwner().HasForeignKey("ProjectId");
                
                successCriteria.Property<Guid>("Id").ValueGeneratedNever();
                successCriteria.HasKey("Id");

                successCriteria.Property(sc => sc.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                successCriteria.Property(sc => sc.Description)
                    .HasMaxLength(2000);

                successCriteria.Property(sc => sc.CriterionType)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                successCriteria.Property(sc => sc.Criticality)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                successCriteria.Property(sc => sc.MeasurementMethod)
                    .HasMaxLength(500);

                successCriteria.Property(sc => sc.TargetValue)
                    .HasMaxLength(500);

                successCriteria.Property(sc => sc.ActualValue)
                    .HasMaxLength(500);

                successCriteria.Property(sc => sc.Unit)
                    .HasMaxLength(50);

                successCriteria.Property(sc => sc.TargetNumeric)
                    .HasPrecision(18, 2);

                successCriteria.Property(sc => sc.ActualNumeric)
                    .HasPrecision(18, 2);

                successCriteria.Property(sc => sc.Weight)
                    .IsRequired()
                    .HasDefaultValue(1.0)
                    .HasPrecision(5, 2);

                successCriteria.Property(sc => sc.OrderIndex)
                    .IsRequired()
                    .HasDefaultValue(0);

                successCriteria.Property(sc => sc.IsMet)
                    .IsRequired()
                    .HasDefaultValue(false);

                successCriteria.Property(sc => sc.TargetDate)
                    .IsRequired(false);

                successCriteria.Property(sc => sc.MetAt)
                    .IsRequired(false);

                successCriteria.Property(sc => sc.MetBy)
                    .HasMaxLength(256);

                successCriteria.Property(sc => sc.Notes)
                    .HasMaxLength(5000);

                successCriteria.Property(sc => sc.EvidenceUrl)
                    .HasMaxLength(2000);

                // AttachmentIds - stored as JSON
                successCriteria.Property("_attachmentIds")
                    .HasColumnName("AttachmentIds")
                    .HasColumnType("jsonb");

                // Metadata - stored as JSON
                successCriteria.Property("_metadata")
                    .HasColumnName("Metadata")
                    .HasColumnType("jsonb");

                successCriteria.ConfigureByConvention();
            });

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(Project.ExtraProperties));

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40);

            // === INDEXES ===
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.TenantId);
            builder.HasIndex(x => x.AreaId);
            builder.HasIndex(x => x.GoalId);
            builder.HasIndex(x => new { x.Status, x.IsArchived });
            builder.HasIndex(x => new { x.OwnerId, x.Status });
        }
    }
}