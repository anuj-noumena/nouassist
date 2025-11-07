using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Tasks.Entities;

namespace NouAssist.Tasks.EntityFrameworkCore.Configurations
{
    public class TaskTypeCustomFieldConfiguration : IEntityTypeConfiguration<TaskTypeCustomField>
    {
        public void Configure(EntityTypeBuilder<TaskTypeCustomField> builder)
        {
            builder.ToTable(TasksDbProperties.DbTablePrefix + "TaskTypeCustomFields",
                          TasksDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.TaskTypeId)
                .IsRequired();

            builder.Property(x => x.CustomFieldDefinitionId)
                .IsRequired();

            builder.Property(x => x.IsRequired)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.DefaultValue)
                .HasMaxLength(500);

            builder.Property(x => x.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.TabName)
                .HasMaxLength(100);

            builder.Property(x => x.SectionName)
                .HasMaxLength(100);

            builder.Property(x => x.ShowIfFieldKey)
                .HasMaxLength(100);

            builder.Property(x => x.ShowIfValue)
                .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Indexes
            builder.HasIndex(x => x.TaskTypeId);
            builder.HasIndex(x => x.CustomFieldDefinitionId);
            builder.HasIndex(x => new { x.TaskTypeId, x.CustomFieldDefinitionId }).IsUnique();
            builder.HasIndex(x => new { x.TaskTypeId, x.DisplayOrder });
        }
    }
}
