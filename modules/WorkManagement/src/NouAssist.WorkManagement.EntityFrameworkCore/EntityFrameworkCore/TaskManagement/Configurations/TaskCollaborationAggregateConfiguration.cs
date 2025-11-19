using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Configurations
{
    public class TaskCollaborationAggregateConfiguration : IEntityTypeConfiguration<TaskCollaborationAggregate>
    {
        public void Configure(EntityTypeBuilder<TaskCollaborationAggregate> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(WorkManagementDbProperties.DbTablePrefix + "TaskCollaborations",
                          WorkManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Foreign Key to TaskInstance
            builder.Property(x => x.TaskInstanceId)
                .IsRequired();

            // Collaboration Properties
            builder.Property(x => x.CommentCount)
                .HasColumnName("CommentCount")
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.AttachmentCount)
                .HasColumnName("AttachmentCount")
                .IsRequired()
                .HasDefaultValue(0);

            // Watchers stored as JSON (accessing private field)
            builder.Property<HashSet<Guid>>("_watchers")
                .HasColumnName("Watchers")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<HashSet<Guid>>(v, (System.Text.Json.JsonSerializerOptions)null)
                )
                .HasColumnType("jsonb");

            // Indexes
            builder.HasIndex(x => x.TaskInstanceId)
                .IsUnique(); // One-to-one relationship with TaskInstance

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40);
        }
    }
}
