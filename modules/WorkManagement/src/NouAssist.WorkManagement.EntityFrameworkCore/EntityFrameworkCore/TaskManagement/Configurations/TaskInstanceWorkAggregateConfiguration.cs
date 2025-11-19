using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Configurations
{
    public class TaskInstanceWorkAggregateConfiguration : IEntityTypeConfiguration<TaskInstanceWorkAggregate>
    {
        public void Configure(EntityTypeBuilder<TaskInstanceWorkAggregate> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(WorkManagementDbProperties.DbTablePrefix + "TaskInstanceWork",
                          WorkManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Foreign Key to TaskInstance
            builder.Property(x => x.TaskInstanceId)
                .IsRequired();

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

            // Indexes
            builder.HasIndex(x => x.TaskInstanceId)
                .IsUnique(); // One-to-one relationship with TaskInstance

            // Indexes on owned entity properties (using EF Core shadow properties)
            builder.HasIndex("WorkTracking_StartedAt")
                .HasDatabaseName("IX_TaskInstanceWork_StartedAt");

            builder.HasIndex("WorkTracking_CompletedAt")
                .HasDatabaseName("IX_TaskInstanceWork_CompletedAt");

            builder.HasIndex("Schedule_ActualDueDate")
                .HasDatabaseName("IX_TaskInstanceWork_ActualDueDate");

            builder.HasIndex("Schedule_PlannedDueDate")
                .HasDatabaseName("IX_TaskInstanceWork_PlannedDueDate");

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40);
        }
    }
}
