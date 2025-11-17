using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Configurations
{
    public class TaskDependencyAggregateConfiguration : IEntityTypeConfiguration<TaskDependencyAggregate>
    {
        public void Configure(EntityTypeBuilder<TaskDependencyAggregate> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(TasksDbProperties.DbTablePrefix + "TaskDependencyAggregates",
                          TasksDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.TaskInstanceId)
                .IsRequired();

            builder.Property(x => x.TenantId);

            // Owned collection: Dependencies
            builder.OwnsMany(x => x.Dependencies, dependencyBuilder =>
            {
                dependencyBuilder.ToTable(TasksDbProperties.DbTablePrefix + "TaskDependencyItems",
                    TasksDbProperties.DbSchema);

                dependencyBuilder.Property(d => d.DependsOnTaskInstanceId)
                    .IsRequired()
                    .HasColumnName("DependsOnTaskInstanceId");

                dependencyBuilder.Property(d => d.DependencyType)
                    .IsRequired()
                    .HasColumnName("DependencyType");

                dependencyBuilder.Property(d => d.LagMinutes)
                    .HasColumnName("LagMinutes")
                    .HasDefaultValue(0);

                dependencyBuilder.Property(d => d.IsBlocking)
                    .HasColumnName("IsBlocking")
                    .HasDefaultValue(true);

                dependencyBuilder.HasIndex(d => d.DependsOnTaskInstanceId);
            });

            // Indexes
            builder.HasIndex(x => x.TaskInstanceId)
                .IsUnique();

            builder.HasIndex(x => x.TenantId);
        }
    }
}
