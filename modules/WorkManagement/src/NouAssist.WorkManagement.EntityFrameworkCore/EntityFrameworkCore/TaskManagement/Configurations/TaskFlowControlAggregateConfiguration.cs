using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Configurations
{
    public class TaskFlowControlAggregateConfiguration : IEntityTypeConfiguration<TaskFlowControlAggregate>
    {
        public void Configure(EntityTypeBuilder<TaskFlowControlAggregate> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(WorkManagementDbProperties.DbTablePrefix + "TaskFlowControls",
                          WorkManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Foreign Key to TaskInstance
            builder.Property(x => x.TaskInstanceId)
                .IsRequired();

            // Approval
            builder.Property(x => x.ApprovalRequestId);

            // Blocking/Flagging - stored as private fields, exposed via properties
            builder.Property<string>("_blockingReason")
                .HasColumnName("BlockedReason")
                .HasMaxLength(TasksDbConsts.MaxBlockedReasonLength);

            builder.Property<DateTime?>("_blockedAt")
                .HasColumnName("BlockedAt");

            builder.Property<string>("_flagReason")
                .HasColumnName("FlaggedReason")
                .HasMaxLength(TasksDbConsts.MaxFlaggedReasonLength);

            // Relationships
            builder.HasMany(x => x.Dependencies)
                .WithOne()
                .HasForeignKey("TaskFlowControlAggregateId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.TaskInstanceId)
                .IsUnique(); // One-to-one relationship with TaskInstance

            builder.HasIndex(x => x.ApprovalRequestId);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40);
        }
    }
}
