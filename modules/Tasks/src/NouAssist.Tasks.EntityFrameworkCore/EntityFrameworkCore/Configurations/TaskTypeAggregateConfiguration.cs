using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Tasks.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Tasks.EntityFrameworkCore.Configurations
{
    public class TaskTypeAggregateConfiguration : IEntityTypeConfiguration<TaskType>
    {
        public void Configure(EntityTypeBuilder<TaskType> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(TasksDbProperties.DbTablePrefix + "TaskTypes",
                          TasksDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(TasksDbConsts.MaxTaskTypeNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(TasksDbConsts.MaxTaskTypeDescriptionLength);

            builder.Property(x => x.Color)
                .HasMaxLength(TasksDbConsts.MaxColorLength);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Relationships
            builder.HasMany(x => x.CustomFields)
                .WithOne()
                .HasForeignKey("TaskTypeId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => new { x.TenantId, x.Name }).IsUnique();

            // Multi-tenancy
            builder.HasIndex(x => x.TenantId);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40);
        }
    }
}
