using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Configurations
{
    public class TaskTemplateConfiguration : IEntityTypeConfiguration<TaskTemplate>
    {
        public void Configure(EntityTypeBuilder<TaskTemplate> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(TasksDbProperties.DbTablePrefix + "TaskTemplates",
                          TasksDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.TaskId)
                .IsRequired();

            builder.Property(x => x.TenantId);

            // Child collections
            builder.HasMany(x => x.ChecklistItems)
                .WithOne()
                .HasForeignKey(ci => ci.TaskId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.AcceptanceCriteria)
                .WithOne()
                .HasForeignKey(ac => ac.TaskId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.TaskId)
                .IsUnique();

            builder.HasIndex(x => x.TenantId);
        }
    }
}
