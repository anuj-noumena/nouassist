using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Configurations
{
    public class TaskChecklistAggregateConfiguration : IEntityTypeConfiguration<TaskChecklistAggregate>
    {
        public void Configure(EntityTypeBuilder<TaskChecklistAggregate> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(WorkManagementDbProperties.DbTablePrefix + "TaskChecklists",
                          WorkManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Foreign Key to TaskInstance
            builder.Property(x => x.TaskInstanceId)
                .IsRequired();

            // Relationships
            builder.HasMany(x => x.ChecklistItems)
                .WithOne()
                .HasForeignKey("TaskChecklistAggregateId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.CriterionCompletions)
                .WithOne()
                .HasForeignKey("TaskChecklistAggregateId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.ActionStepCompletions)
                .WithOne()
                .HasForeignKey("TaskChecklistAggregateId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.TaskInstanceId)
                .IsUnique(); // One-to-one relationship with TaskInstance

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40);
        }
    }
}
