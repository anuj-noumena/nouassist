using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.TaskManagement.Aggregates.Task;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Configurations
{
    public class TaskDefinitionAggregateConfiguration : IEntityTypeConfiguration<TaskDefinitionAggregate>
    {
        public void Configure(EntityTypeBuilder<TaskDefinitionAggregate> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(WorkManagementDbProperties.DbTablePrefix + "TaskDefinitions",
                          WorkManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Foreign Key to TaskAggregate
            builder.Property(x => x.TaskId)
                .IsRequired();

            // Relationships
            builder.HasMany(x => x.ChecklistItems)
                .WithOne()
                .HasForeignKey("TaskDefinitionId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.AcceptanceCriteria)
                .WithOne()
                .HasForeignKey("TaskDefinitionId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.ActionSteps)
                .WithOne()
                .HasForeignKey("TaskDefinitionId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.TaskId)
                .IsUnique(); // One-to-one relationship with TaskAggregate

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40);
        }
    }
}
