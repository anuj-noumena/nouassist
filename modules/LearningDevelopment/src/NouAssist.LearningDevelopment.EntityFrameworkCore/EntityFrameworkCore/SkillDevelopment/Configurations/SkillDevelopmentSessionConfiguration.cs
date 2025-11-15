using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.SkillDevelopment.Aggregates;
using NouAssist.SkillDevelopment.Entities;
using NouAssist.SkillDevelopment.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.SkillDevelopment.EntityFrameworkCore.Configurations
{
    public class SkillDevelopmentSessionConfiguration : IEntityTypeConfiguration<SkillDevelopmentSession>
    {
        public void Configure(EntityTypeBuilder<SkillDevelopmentSession> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                SkillDevelopmentDbProperties.DbTablePrefix + "DevelopmentSessions",
                SkillDevelopmentDbProperties.DbSchema);

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
            builder.OwnsMany(x => x.Milestones, milestones =>
            {
                milestones.ToTable(
                    SkillDevelopmentDbProperties.DbTablePrefix + "SessionMilestones",
                    SkillDevelopmentDbProperties.DbSchema);

                milestones.WithOwner().HasForeignKey("SessionId");

                milestones.Property<Guid>("SessionId").IsRequired();
                
                milestones.HasKey("SessionId", "Step");

                // Configure SkillMilestone - see separate configuration below
            });

            // Store PracticeTimeTracking as JSON
            builder.Property(x => x.PracticeTimeTracking)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<DevelopmentStep, int>>(v, (System.Text.Json.JsonSerializerOptions)null))
                .HasColumnName("PracticeTimeTracking")
                .HasColumnType("jsonb");

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
