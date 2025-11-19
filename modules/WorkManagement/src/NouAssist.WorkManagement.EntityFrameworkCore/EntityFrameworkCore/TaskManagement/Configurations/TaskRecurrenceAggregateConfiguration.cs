using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.TaskManagement.Aggregates.Task;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Configurations
{
    public class TaskRecurrenceAggregateConfiguration : IEntityTypeConfiguration<TaskRecurrenceAggregate>
    {
        public void Configure(EntityTypeBuilder<TaskRecurrenceAggregate> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(WorkManagementDbProperties.DbTablePrefix + "TaskRecurrences",
                          WorkManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Foreign Key to TaskAggregate
            builder.Property(x => x.TaskId)
                .IsRequired();

            // Recurrence properties
            builder.Property(x => x.Frequency)
                .HasColumnName("Frequency")
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Interval)
                .HasColumnName("Interval")
                .IsRequired();

            builder.Property(x => x.EndDate)
                .HasColumnName("EndDate");

            builder.Property(x => x.Count)
                .HasColumnName("Count");

            builder.Property(x => x.DaysOfWeek)
                .HasColumnName("DaysOfWeek")
                .HasConversion(
                    v => string.Join(',', v.Select(d => (int)d)),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                          .Select(s => (DayOfWeek)int.Parse(s))
                          .ToList()
                );

            // Indexes
            builder.HasIndex(x => x.TaskId)
                .IsUnique(); // One-to-one relationship with TaskAggregate

            builder.HasIndex(x => x.Frequency);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40);
        }
    }
}
