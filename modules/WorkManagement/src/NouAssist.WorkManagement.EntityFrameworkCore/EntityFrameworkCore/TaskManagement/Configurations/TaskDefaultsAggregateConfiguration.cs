using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.TaskManagement.Aggregates.Task;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Configurations
{
    public class TaskDefaultsAggregateConfiguration : IEntityTypeConfiguration<TaskDefaultsAggregate>
    {
        public void Configure(EntityTypeBuilder<TaskDefaultsAggregate> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(WorkManagementDbProperties.DbTablePrefix + "TaskDefaults",
                          WorkManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Foreign Key to TaskAggregate
            builder.Property(x => x.TaskId)
                .IsRequired();

            // Default values for instances
            builder.Property(x => x.DefaultImportance)
                .IsRequired()
                .HasColumnType("double precision")
                .HasDefaultValue(0.5);

            builder.Property(x => x.DefaultUrgency)
                .IsRequired()
                .HasColumnType("double precision")
                .HasDefaultValue(0.5);

            builder.Property(x => x.DefaultDueDate);

            builder.Property(x => x.DefaultEstimateMinutes);

            builder.Property(x => x.DefaultWorkflowId);

            builder.Property(x => x.DefaultAssigneeId);

            builder.Property(x => x.DefaultAreaId);

            builder.Property(x => x.DefaultContextId);

            builder.Property(x => x.DefaultCognitiveLoad)
                .HasConversion<byte?>()
                .HasColumnName("DefaultCognitiveLoad");

            // Value Object: DefaultComplexityEstimation (owned entity)
            builder.OwnsOne(x => x.DefaultComplexityEstimation, complexityBuilder =>
            {
                complexityBuilder.Property(c => c.Value)
                    .HasColumnName("DefaultComplexityValue")
                    .IsRequired();

                complexityBuilder.Property(c => c.Scale)
                    .HasColumnName("DefaultComplexityScale")
                    .IsRequired()
                    .HasConversion<byte>();
            });

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
