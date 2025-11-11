using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Tasks.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Tasks.EntityFrameworkCore.Configurations
{
    public class TaskInstanceAggregateConfiguration : IEntityTypeConfiguration<TaskInstance>
    {
        public void Configure(EntityTypeBuilder<TaskInstance> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(TasksDbProperties.DbTablePrefix + "TaskInstances",
                          TasksDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.TaskId)
                .IsRequired();

            builder.Property(x => x.ProjectId)
                .IsRequired();

            builder.Property(x => x.WorkflowStatusId)
                .IsRequired();

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.TaskStatus)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ProgressPercent)
                .IsRequired()
                .HasColumnType("double precision");

            builder.Property(x => x.ActualMinutes)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.SubtasksCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.SubtasksDoneCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.CommentsCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.AttachmentsCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.IsBlocked)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.BlockedReason)
                .HasMaxLength(TasksDbConsts.MaxBlockedReasonLength);

            builder.Property(x => x.IsFlagged)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.FlaggedReason)
                .HasMaxLength(TasksDbConsts.MaxFlaggedReasonLength);

            builder.Property(x => x.IsNextAction)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.IsDiscussionLocked)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.RequiresApproval)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.Resolution)
                .HasMaxLength(TasksDbConsts.MaxResolutionLength);

            builder.Property(x => x.ResolutionNote)
                .HasMaxLength(TasksDbConsts.MaxResolutionNoteLength);

            builder.Property(x => x.Version)
                .IsRequired()
                .IsConcurrencyToken();

            builder.Property(x => x.OrderInParent)
                .HasDefaultValue(0);

            // Value Object: ImportanceUrgencyMatrix (owned entity)
            builder.OwnsOne(x => x.PriorityMatrix, priorityBuilder =>
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

            // Value Object: DelegationInfo (owned entity)
            builder.OwnsOne(x => x.DelegationInfo, delegationBuilder =>
            {
                delegationBuilder.Property(d => d.DelegatedBy)
                    .HasColumnName("DelegatedBy")
                    .IsRequired();

                delegationBuilder.Property(d => d.DelegatedTo)
                    .HasColumnName("DelegatedTo")
                    .IsRequired();

                delegationBuilder.Property(d => d.Status)
                    .HasColumnName("DelegationStatus")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                delegationBuilder.Property(d => d.DelegatedAt)
                    .HasColumnName("DelegatedAt")
                    .IsRequired();

                delegationBuilder.Property(d => d.RespondedAt)
                    .HasColumnName("DelegationRespondedAt");

                delegationBuilder.Property(d => d.Note)
                    .HasColumnName("DelegationNote")
                    .HasMaxLength(TasksDbConsts.MaxDelegationNoteLength);

                delegationBuilder.Property(d => d.ResponseNote)
                    .HasColumnName("DelegationResponseNote")
                    .HasMaxLength(TasksDbConsts.MaxDelegationResponseNoteLength);
            });

            // Collection: Watchers (stored as JSON)
            builder.Property(x => x.Watchers)
                .HasColumnName("Watchers")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(v, (System.Text.Json.JsonSerializerOptions)null)
                )
                .HasColumnType("jsonb");

            // Custom Field Values (JSON)
            builder.Property(x => x.CustomFieldValues)
                .HasColumnName("CustomFieldValues")
                .HasColumnType("jsonb");

            // Relationships
            builder.HasMany(x => x.StepCompletions)
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
            builder.HasIndex(x => x.WorkflowStatusId);
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.AssigneeId);
            builder.HasIndex(x => x.TaskStatus);
            builder.HasIndex(x => x.IsBlocked);
            builder.HasIndex(x => x.IsNextAction);
            builder.HasIndex(x => x.CompletedAt);
            builder.HasIndex(x => x.ActualDueDate);
            builder.HasIndex(x => x.PlannedDueDate);
            builder.HasIndex(x => new { x.TenantId, x.IsDeleted });
            builder.HasIndex(x => x.ParentInstanceId);

            // Multi-tenancy
            builder.HasIndex(x => x.TenantId);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40);
        }
    }
}
