using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Configurations
{
    public class TaskInstanceAggregateConfiguration : IEntityTypeConfiguration<TaskInstance>
    {
        public void Configure(EntityTypeBuilder<TaskInstance> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(TasksDbProperties.DbTablePrefix + "TaskInstances",
                          TasksDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Core Identity
            builder.Property(x => x.TaskId)
                .IsRequired();

            builder.Property(x => x.WorkflowId)
                .IsRequired();

            builder.Property(x => x.ProjectId);

            builder.Property(x => x.EntityVersion)
                .IsRequired()
                .HasDefaultValue(1);

            // Ownership
            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.AssigneeId);

            builder.Property(x => x.ReporterId);

            builder.Property(x => x.TeamId);

            // GTD Organization
            builder.Property(x => x.AreaId);

            builder.Property(x => x.ContextId);

            // Workflow State
            builder.Property(x => x.WorkflowStatusId)
                .IsRequired();

            // Approval
            builder.Property(x => x.ApprovalRequestId);

            // Value Object: TaskWorkTracking (owned entity)
            builder.OwnsOne(x => x.WorkTracking, workBuilder =>
            {
                workBuilder.Property(w => w.ProgressPercent)
                    .HasColumnName("ProgressPercent")
                    .IsRequired()
                    .HasColumnType("double precision")
                    .HasDefaultValue(0.0);

                workBuilder.Property(w => w.StartedAt)
                    .HasColumnName("StartedAt");

                workBuilder.Property(w => w.CompletedAt)
                    .HasColumnName("CompletedAt");

                workBuilder.Property(w => w.ActualMinutes)
                    .HasColumnName("ActualMinutes")
                    .IsRequired()
                    .HasDefaultValue(0);

                workBuilder.Property(w => w.EstimatedMinutes)
                    .HasColumnName("EstimatedMinutes");

                workBuilder.Property(w => w.IsNextAction)
                    .HasColumnName("IsNextAction")
                    .IsRequired()
                    .HasDefaultValue(false);
            });

            // Value Object: TaskSchedule (owned entity)
            builder.OwnsOne(x => x.Schedule, scheduleBuilder =>
            {
                scheduleBuilder.Property(s => s.PlannedStartDate)
                    .HasColumnName("PlannedStartDate");

                scheduleBuilder.Property(s => s.PlannedDueDate)
                    .HasColumnName("PlannedDueDate");

                scheduleBuilder.Property(s => s.ActualStartDate)
                    .HasColumnName("ActualStartDate");

                scheduleBuilder.Property(s => s.ActualDueDate)
                    .HasColumnName("ActualDueDate");

                scheduleBuilder.Property(s => s.CompletedAt)
                    .HasColumnName("CompletedAt");
            });

            // Value Object: ImportanceUrgencyMatrix (owned entity)
            builder.OwnsOne(x => x.Priority, priorityBuilder =>
            {
                priorityBuilder.Property(p => p.Importance)
                    .HasColumnName("Importance")
                    .IsRequired()
                    .HasColumnType("double precision");

                priorityBuilder.Property(p => p.Urgency)
                    .HasColumnName("Urgency")
                    .IsRequired()
                    .HasColumnType("double precision");

                priorityBuilder.Property(p => p.Quadrant)
                    .HasColumnName("EisenhowerQuadrant")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);
            });

            // Value Object: ComplexityEstimation (owned entity)
            builder.OwnsOne(x => x.ComplexityEstimation, complexityBuilder =>
            {
                complexityBuilder.Property(c => c.Value)
                    .HasColumnName("ComplexityValue")
                    .IsRequired();

                complexityBuilder.Property(c => c.Scale)
                    .HasColumnName("ComplexityScale")
                    .IsRequired()
                    .HasConversion<byte>();
            });

            // CognitiveLoad
            builder.Property(x => x.CognitiveLoad)
                .HasConversion<byte?>()
                .HasColumnName("CognitiveLoad");

            // Value Object: TaskCollaboration (owned entity)
            builder.OwnsOne(x => x.Collaboration, collabBuilder =>
            {
                collabBuilder.Property(c => c.CommentCount)
                    .HasColumnName("CommentsCount")
                    .IsRequired()
                    .HasDefaultValue(0);

                collabBuilder.Property(c => c.AttachmentCount)
                    .HasColumnName("AttachmentsCount")
                    .IsRequired()
                    .HasDefaultValue(0);

                // Watchers stored as JSON
                collabBuilder.Property(c => c.Watchers)
                    .HasColumnName("Watchers")
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(v, (System.Text.Json.JsonSerializerOptions)null)
                    )
                    .HasColumnType("jsonb");
            });

            // Custom Field Values (JSON)
            builder.OwnsOne(x => x.CustomFields, customBuilder =>
            {
                customBuilder.Property<string>("_values")
                    .HasColumnName("CustomFieldValues")
                    .HasColumnType("jsonb");
            });

            // Blocking/Flagging - stored as private fields, exposed via properties
            builder.Property<string>("_blockingReason")
                .HasColumnName("BlockedReason")
                .HasMaxLength(TasksDbConsts.MaxBlockedReasonLength);

            builder.Property<DateTime?>("_blockedAt")
                .HasColumnName("BlockedAt");

            builder.Property<string>("_flagReason")
                .HasColumnName("FlaggedReason")
                .HasMaxLength(TasksDbConsts.MaxFlaggedReasonLength);

            // Resolution - stored as private fields
            builder.Property<string>("_resolution")
                .HasColumnName("Resolution")
                .HasMaxLength(TasksDbConsts.MaxResolutionLength);

            builder.Property<string>("_resolutionNote")
                .HasColumnName("ResolutionNote")
                .HasMaxLength(TasksDbConsts.MaxResolutionNoteLength);

            // Relationships
            builder.HasMany(x => x.ChecklistItems)
                .WithOne()
                .HasForeignKey("TaskInstanceId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.CriterionCompletions)
                .WithOne()
                .HasForeignKey("TaskInstanceId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Dependencies)
                .WithOne()
                .HasForeignKey("TaskInstanceId")
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.TaskId);
            builder.HasIndex(x => x.ProjectId);
            builder.HasIndex(x => x.AreaId);
            builder.HasIndex(x => x.ContextId);
            builder.HasIndex(x => x.WorkflowId);
            builder.HasIndex(x => x.WorkflowStatusId);
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.AssigneeId);
            builder.HasIndex(x => new { x.TenantId, x.IsDeleted });
            builder.HasIndex(x => new { x.AreaId, x.IsDeleted });
            builder.HasIndex(x => new { x.ProjectId, x.IsDeleted });

            // Indexes on owned entity properties (using backing field access)
            builder.HasIndex("WorkTracking_StartedAt")
                .HasDatabaseName("IX_TaskInstances_StartedAt");
            
            builder.HasIndex("WorkTracking_CompletedAt")
                .HasDatabaseName("IX_TaskInstances_CompletedAt");
            
            builder.HasIndex("Schedule_ActualDueDate")
                .HasDatabaseName("IX_TaskInstances_ActualDueDate");
            
            builder.HasIndex("Schedule_PlannedDueDate")
                .HasDatabaseName("IX_TaskInstances_PlannedDueDate");

            builder.HasIndex("Hierarchy_ParentInstanceId")
                .HasDatabaseName("IX_TaskInstances_ParentInstanceId");

            // Multi-tenancy
            builder.HasIndex(x => x.TenantId);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40);
        }
    }
}