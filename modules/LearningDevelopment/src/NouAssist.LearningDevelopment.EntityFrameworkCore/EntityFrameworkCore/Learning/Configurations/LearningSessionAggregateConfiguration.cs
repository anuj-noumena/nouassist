using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using NouAssist.LearningDevelopment.Learning.Entities;
using System.Collections.Generic;
using System.Text.Json;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore.Learning.Configurations
{
    public class LearningSessionAggregateConfiguration : IEntityTypeConfiguration<LearningSession>
    {
        public void Configure(EntityTypeBuilder<LearningSession> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                LearningDevelopmentDbProperties.DbTablePrefix + "LearningSessions",
                LearningDevelopmentDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.UserId)
                .IsRequired()
                .HasColumnName(nameof(LearningSession.UserId));

            builder.Property(x => x.TopicId)
                .IsRequired()
                .HasColumnName(nameof(LearningSession.TopicId));

            builder.Property(x => x.CurrentPhase)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasColumnName(nameof(LearningSession.CurrentPhase));

            builder.Property(x => x.ReviewCycle)
                .IsRequired()
                .HasColumnName(nameof(LearningSession.ReviewCycle));

            builder.Property(x => x.Completed)
                .IsRequired()
                .HasColumnName(nameof(LearningSession.Completed));

            builder.Property(x => x.Paused)
                .IsRequired()
                .HasColumnName(nameof(LearningSession.Paused));

            builder.Property(x => x.PausedAt)
                .HasColumnName(nameof(LearningSession.PausedAt));

            builder.Property(x => x.ResumedAt)
                .HasColumnName(nameof(LearningSession.ResumedAt));

            builder.Property(x => x.LastActivity)
                .IsRequired()
                .HasColumnName(nameof(LearningSession.LastActivity));

            // Complex Type: TimeTracking (stored as JSON)
            var timeTrackingConverter = new ValueConverter<Dictionary<int, int>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<Dictionary<int, int>>(v, (JsonSerializerOptions)null));

            builder.Property("_timeTracking")
                .HasColumnName("TimeTracking")
                .HasColumnType("jsonb")
                .HasConversion(timeTrackingConverter);

            // Owned collection: PhaseMilestones
            builder.HasMany<PhaseMilestone>()
                .WithOne()
                .HasForeignKey(pm => pm.SessionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_Learning_LearningSessions_UserId");

            builder.HasIndex(x => x.TopicId)
                .IsUnique()
                .HasDatabaseName("IX_Learning_LearningSessions_TopicId");

            builder.HasIndex(x => new { x.UserId, x.Completed })
                .HasDatabaseName("IX_Learning_LearningSessions_UserId_Completed");

            builder.HasIndex(x => new { x.UserId, x.Paused })
                .HasDatabaseName("IX_Learning_LearningSessions_UserId_Paused");

            builder.HasIndex(x => x.LastActivity)
                .HasDatabaseName("IX_Learning_LearningSessions_LastActivity");

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(LearningSession.ExtraProperties))
                .HasColumnType("jsonb");

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40)
                .HasColumnName(nameof(LearningSession.ConcurrencyStamp));
        }
    }
}
