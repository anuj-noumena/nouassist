using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.LearningDevelopment.Integration.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore.Integration.Configurations
{
    public class LearningActivityConfiguration : IEntityTypeConfiguration<LearningActivity>
    {
        public void Configure(EntityTypeBuilder<LearningActivity> builder)
        {
            builder.ToTable("LearningActivities");

            builder.ConfigureByConvention();

            // Properties
            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.LearningPathId)
                .IsRequired();

            builder.Property(x => x.TotalTasksCreated)
                .IsRequired();

            builder.Property(x => x.TheoryTasksCompleted)
                .IsRequired();

            builder.Property(x => x.PracticeTasksCompleted)
                .IsRequired();

            builder.Property(x => x.ScheduledMinutes)
                .IsRequired();

            builder.Property(x => x.ActualMinutesSpent)
                .IsRequired();

            builder.Property(x => x.OverallProgress)
                .IsRequired();

            builder.Property(x => x.LastSyncAt)
                .IsRequired();

            // Indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.LearningPathId)
                .IsUnique();
            builder.HasIndex(x => new { x.UserId, x.OverallProgress });
            builder.HasIndex(x => x.LastSyncAt);

            // Collections stored as JSON
            builder.Property(x => x.TaskInstanceIds)
                .HasConversion(
                    v => string.Join(",", v),
                    v => v.Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                        .Select(System.Guid.Parse)
                        .ToList())
                .HasColumnName("TaskInstanceIds")
                .HasMaxLength(10000);

            builder.Property(x => x.TimeblockIds)
                .HasConversion(
                    v => string.Join(",", v),
                    v => v.Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                        .Select(System.Guid.Parse)
                        .ToList())
                .HasColumnName("TimeblockIds")
                .HasMaxLength(10000);

            builder.Property(x => x.TimeEntryIds)
                .HasConversion(
                    v => string.Join(",", v),
                    v => v.Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                        .Select(System.Guid.Parse)
                        .ToList())
                .HasColumnName("TimeEntryIds")
                .HasMaxLength(10000);
        }
    }
}
