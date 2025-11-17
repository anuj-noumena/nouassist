using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.TaskManagement.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Configurations
{
    public class TaskChecklistItemConfiguration : IEntityTypeConfiguration<TaskChecklistItem>
    {
        public void Configure(EntityTypeBuilder<TaskChecklistItem> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(WorkManagementDbProperties.DbTablePrefix + "TaskChecklistItems",
                WorkManagementDbProperties.DbSchema);

            // Composite key
            builder.HasKey(x => new { x.TaskInstanceId, x.ChecklistItemId });

            // Properties
            builder.Property(x => x.TaskInstanceId)
                .IsRequired();

            builder.Property(x => x.ChecklistItemId)
                .IsRequired();

            builder.Property(x => x.IsChecked)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.CheckedAt);

            builder.Property(x => x.CheckedBy);

            // Indexes
            builder.HasIndex(x => x.TaskInstanceId);
            builder.HasIndex(x => x.ChecklistItemId);
            builder.HasIndex(x => new { x.TaskInstanceId, x.IsChecked });
        }
    }
}