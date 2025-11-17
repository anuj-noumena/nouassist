using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;
using System.Text.Json;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Configurations
{
    public class TaskCollaborationAggregateConfiguration : IEntityTypeConfiguration<TaskCollaborationAggregate>
    {
        public void Configure(EntityTypeBuilder<TaskCollaborationAggregate> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(TasksDbProperties.DbTablePrefix + "TaskCollaborationAggregates",
                          TasksDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.TaskInstanceId)
                .IsRequired();

            builder.Property(x => x.TenantId);

            // Watchers stored as JSON array
            builder.Property(x => x.Watchers)
                .HasColumnName("Watchers")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<System.Collections.Generic.List<System.Guid>>(v, (JsonSerializerOptions)null)
                );

            builder.Property(x => x.CommentCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.AttachmentCount)
                .IsRequired()
                .HasDefaultValue(0);

            // Indexes
            builder.HasIndex(x => x.TaskInstanceId)
                .IsUnique();

            builder.HasIndex(x => x.TenantId);
        }
    }
}
