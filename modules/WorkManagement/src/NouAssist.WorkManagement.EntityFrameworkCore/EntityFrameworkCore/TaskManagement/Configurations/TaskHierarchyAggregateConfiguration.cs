using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.TaskManagement.Aggregates.Task;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Configurations
{
    public class TaskHierarchyAggregateConfiguration : IEntityTypeConfiguration<TaskHierarchyAggregate>
    {
        public void Configure(EntityTypeBuilder<TaskHierarchyAggregate> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(WorkManagementDbProperties.DbTablePrefix + "TaskHierarchies",
                          WorkManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Foreign Key to TaskAggregate
            builder.Property(x => x.TaskId)
                .IsRequired();

            // Hierarchy properties
            builder.Property(x => x.ParentId);

            builder.Property(x => x.Level)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.Path)
                .IsRequired()
                .HasMaxLength(1000)
                .HasDefaultValue("/");

            // Indexes
            builder.HasIndex(x => x.TaskId)
                .IsUnique(); // One-to-one relationship with TaskAggregate

            builder.HasIndex(x => x.ParentId);
            builder.HasIndex(x => x.Level);
            builder.HasIndex(x => x.Path);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40);
        }
    }
}
