using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Configurations
{
    public class TaskInstanceAggregateConfiguration : IEntityTypeConfiguration<TaskInstance>
    {
        public void Configure(EntityTypeBuilder<TaskInstance> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(WorkManagementDbProperties.DbTablePrefix + "TaskInstances",
                          WorkManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Core Identity
            builder.Property(x => x.TaskId)
                .IsRequired();

            builder.Property(x => x.WorkflowId)
                .IsRequired();

            builder.Property(x => x.WorkflowStatusId)
                .IsRequired();

            builder.Property(x => x.EntityVersion)
                .IsRequired()
                .HasDefaultValue(1);

            // Ownership
            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.AssigneeId);

            builder.Property(x => x.ReporterId);

            builder.Property(x => x.TeamId);

            // Core Properties
            builder.Property(x => x.Notes)
                .HasMaxLength(TasksDbConsts.MaxNotesLength);

            // Custom Field Values (owned entity)
            builder.OwnsOne(x => x.CustomFields, customBuilder =>
            {
                customBuilder.Property<string>("_values")
                    .HasColumnName("CustomFieldValues")
                    .HasColumnType("jsonb");
            });

            // Resolution - stored as private fields
            builder.Property<string>("_resolution")
                .HasColumnName("Resolution")
                .HasMaxLength(TasksDbConsts.MaxResolutionLength);

            builder.Property<string>("_resolutionNote")
                .HasColumnName("ResolutionNote")
                .HasMaxLength(TasksDbConsts.MaxResolutionNoteLength);

            // Cancellation - stored as private fields
            builder.Property<bool>("_isCancelled")
                .HasColumnName("IsCancelled")
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property<string>("_cancellationReason")
                .HasColumnName("CancellationReason")
                .HasMaxLength(TasksDbConsts.MaxCancellationReasonLength);

            builder.Property<DateTime?>("_cancelledAt")
                .HasColumnName("CancelledAt");

            // Indexes
            builder.HasIndex(x => x.TaskId);
            builder.HasIndex(x => x.WorkflowId);
            builder.HasIndex(x => x.WorkflowStatusId);
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.AssigneeId);
            builder.HasIndex(x => x.TenantId);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40);
        }
    }
}