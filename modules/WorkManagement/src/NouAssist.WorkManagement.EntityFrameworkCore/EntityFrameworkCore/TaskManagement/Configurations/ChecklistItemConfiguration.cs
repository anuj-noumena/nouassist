using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.TaskManagement.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Configurations
{
    public class ChecklistItemConfiguration : IEntityTypeConfiguration<ChecklistItem>
    {
        public void Configure(EntityTypeBuilder<ChecklistItem> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(TasksDbProperties.DbTablePrefix + "ChecklistItems",
                TasksDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.TaskId)
                .IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(TasksDbConsts.MaxChecklistItemTitleLength);

            builder.Property(x => x.Description)
                .HasMaxLength(TasksDbConsts.MaxChecklistItemDescriptionLength);

            builder.Property(x => x.OrderIndex)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.IsRequired)
                .IsRequired()
                .HasDefaultValue(false);

            // Indexes
            builder.HasIndex(x => x.TaskId);
            builder.HasIndex(x => new { x.TaskId, x.OrderIndex });
        }
    }
}