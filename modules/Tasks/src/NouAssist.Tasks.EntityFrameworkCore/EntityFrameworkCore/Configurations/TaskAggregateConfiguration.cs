using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Tasks.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Tasks.EntityFrameworkCore.Configurations
{
    public class TaskAggregateConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(TasksDbProperties.DbTablePrefix + "Tasks", 
                          TasksDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(TasksDbConsts.MaxTitleLength);

            builder.Property(x => x.Description)
                .HasMaxLength(TasksDbConsts.MaxDescriptionLength);

            builder.Property(x => x.Notes)
                .HasMaxLength(TasksDbConsts.MaxNotesLength);

            builder.Property(x => x.ExpectedResult)
                .HasMaxLength(TasksDbConsts.MaxExpectedResultLength);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.DefaultImportance)
                .IsRequired()
                .HasColumnType("double precision");

            builder.Property(x => x.DefaultUrgency)
                .IsRequired()
                .HasColumnType("double precision");

            builder.Property(x => x.IsRecurring)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.Version)
                .IsRequired()
                .IsConcurrencyToken();

            builder.Property(x => x.EstimateMinutes)
                .HasColumnType("integer");

            // Value Object: RecurrenceRule (owned entity)
            builder.OwnsOne(x => x.RecurrenceRule, recurrenceBuilder =>
            {
                recurrenceBuilder.Property(r => r.Frequency)
                    .HasColumnName("RecurrenceFrequency")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                recurrenceBuilder.Property(r => r.Interval)
                    .HasColumnName("RecurrenceInterval")
                    .IsRequired();

                recurrenceBuilder.Property(r => r.EndDate)
                    .HasColumnName("RecurrenceEndDate");

                recurrenceBuilder.Property(r => r.Count)
                    .HasColumnName("RecurrenceCount");

                recurrenceBuilder.Property(r => r.DaysOfWeek)
                    .HasColumnName("RecurrenceDaysOfWeek")
                    .HasConversion(
                        v => string.Join(',', v.Select(d => (int)d)),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                              .Select(s => (DayOfWeek)int.Parse(s))
                              .ToList()
                    );
            });

            // Relationships
            builder.HasMany(x => x.ActionSteps)
                .WithOne()
                .HasForeignKey("TaskId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.AcceptanceCriteria)
                .WithOne()
                .HasForeignKey("TaskId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.Title);
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.ProjectId);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.IsRecurring);
            builder.HasIndex(x => new { x.TenantId, x.IsDeleted });

            // Multi-tenancy
            builder.HasIndex(x => x.TenantId);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40);
        }
    }
}
