using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.LearningDevelopment.SkillDevelopment.Aggregates;
using NouAssist.LearningDevelopment.SkillDevelopment.Entities;
using NouAssist.SkillDevelopment;
using NouAssist.SkillDevelopment.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore.SkillDevelopment.Configurations
{
    public class SkillDevelopmentSessionConfiguration : IEntityTypeConfiguration<SkillDevelopmentSession>
    {
        public void Configure(EntityTypeBuilder<SkillDevelopmentSession> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                LearningDevelopmentDbProperties.DbTablePrefix + "DevelopmentSessions",
                LearningDevelopmentDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.SkillId)
                .IsRequired();

            builder.Property(x => x.CurrentStep)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ReviewCycle)
                .IsRequired();

            builder.Property(x => x.Completed)
                .IsRequired();

            builder.Property(x => x.Paused)
                .IsRequired();

            builder.Property(x => x.LastActivity)
                .IsRequired();

            // Store Milestones as owned entities (separate table)
            builder.OwnsMany<SkillMilestone>(x => x.Milestones.Values, milestones =>
            {
                milestones.ToTable(
                    LearningDevelopmentDbProperties.DbTablePrefix + "SessionMilestones",
                    LearningDevelopmentDbProperties.DbSchema);

                milestones.WithOwner().HasForeignKey("SessionId");

                milestones.Property<Guid>("SessionId").IsRequired();
                
                milestones.HasKey("SessionId", "Step");

                // Configure SkillMilestone - see separate configuration below
            });

            // Store PracticeTimeTracking as JSON
            var practiceTimeTrackingConverter = new ValueConverter<Dictionary<DevelopmentStep, int>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<Dictionary<DevelopmentStep, int>>(v, (JsonSerializerOptions)null));

            builder.Property("_practiceTimeTracking")
                .HasColumnName("PracticeTimeTracking")
                .HasColumnType("jsonb")
                .HasConversion(practiceTimeTrackingConverter);

            // Indexes
            builder.HasIndex(x => x.SkillId).IsUnique();
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => new { x.UserId, x.Completed });
            builder.HasIndex(x => x.LastActivity);

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(SkillDevelopmentSession.ExtraProperties));

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}