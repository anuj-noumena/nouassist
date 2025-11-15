using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Learning.Aggregates;
using NouAssist.Learning.Entities;
using NouAssist.Learning.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Learning.EntityFrameworkCore.Configurations
{
    public class LearningSessionAggregateConfiguration : IEntityTypeConfiguration<LearningSession>
    {
        public void Configure(EntityTypeBuilder<LearningSession> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                LearningDbProperties.DbTablePrefix + "LearningSessions",
                LearningDbProperties.DbSchema);

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
            builder.Property("_timeTracking")
                .HasColumnName("TimeTracking")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<int, int>>(v, (System.Text.Json.JsonSerializerOptions)null));

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
