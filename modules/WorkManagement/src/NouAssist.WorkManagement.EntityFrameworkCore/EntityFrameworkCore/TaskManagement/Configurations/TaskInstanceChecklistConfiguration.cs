using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Configurations
{
    public class TaskInstanceChecklistConfiguration : IEntityTypeConfiguration<TaskInstanceChecklist>
    {
        public void Configure(EntityTypeBuilder<TaskInstanceChecklist> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(TasksDbProperties.DbTablePrefix + "TaskInstanceChecklists",
                          TasksDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.TaskInstanceId)
                .IsRequired();

            builder.Property(x => x.TenantId);

            // Child collections
            builder.HasMany(x => x.ChecklistItems)
                .WithOne()
                .HasForeignKey(ci => ci.TaskInstanceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.TaskInstanceId)
                .IsUnique();

            builder.HasIndex(x => x.TenantId);
        }
    }
}
