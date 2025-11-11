using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Tasks.Entities;

namespace NouAssist.Tasks.EntityFrameworkCore.Configurations
{
    public class TaskDependencyConfiguration : IEntityTypeConfiguration<TaskDependency>
    {
        public void Configure(EntityTypeBuilder<TaskDependency> builder)
        {
            builder.ToTable(TasksDbProperties.DbTablePrefix + "TaskDependencies",
                          TasksDbProperties.DbSchema);

            // Composite key based on dependency type
            builder.HasKey(x => new { x.TaskId, x.TaskInstanceId, x.DependsOnTaskId, x.DependsOnTaskInstanceId });

            // Properties
            builder.Property(x => x.DependencyType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.LagMinutes)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.IsBlocking)
                .IsRequired()
                .HasDefaultValue(true);

            // Indexes
            builder.HasIndex(x => x.TaskId);
            builder.HasIndex(x => x.TaskInstanceId);
            builder.HasIndex(x => x.DependsOnTaskId);
            builder.HasIndex(x => x.DependsOnTaskInstanceId);
            builder.HasIndex(x => new { x.TaskInstanceId, x.IsBlocking });
        }
    }
}
