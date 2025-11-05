using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Tasks.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.EntityFrameworkCore.Tasks.EntityFrameworkCore.Configurations
{
    public class TaskActionStepCompletionConfiguration : IEntityTypeConfiguration<TaskActionStepCompletion>
    {
        public void Configure(EntityTypeBuilder<TaskActionStepCompletion> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(TasksDbProperties.DbTablePrefix + "TaskActionStepCompletions",
                          TasksDbProperties.DbSchema);

            // Composite key
            builder.HasKey(x => new { x.TaskInstanceId, x.TaskActionStepId });

            // Properties
            builder.Property(x => x.TaskInstanceId)
                .IsRequired();

            builder.Property(x => x.TaskActionStepId)
                .IsRequired();

            builder.Property(x => x.IsDone)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.CompletedAt);

            builder.Property(x => x.CompletedBy);

            // Indexes
            builder.HasIndex(x => x.TaskInstanceId);
            builder.HasIndex(x => x.TaskActionStepId);
            builder.HasIndex(x => new { x.TaskInstanceId, x.IsDone });

            // Concurrency
            // builder.Property(x => x.ConcurrencyStamp)
            //     .IsConcurrencyToken()
            //     .HasMaxLength(40);
        }
    }
}
