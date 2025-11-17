using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.LearningDevelopment.SkillDevelopment.Entities;
using NouAssist.LearningDevelopment.SkillDevelopment.ValueObjects;
using NouAssist.SkillDevelopment;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore.SkillDevelopment.Configurations
{
    public class SkillMilestoneConfiguration : IEntityTypeConfiguration<SkillMilestone>
    {
        public void Configure(EntityTypeBuilder<SkillMilestone> builder)
        {
            // Note: This is called within the OwnsMany configuration in SkillDevelopmentSessionConfiguration
            // So table and key are already configured

            builder.Property(x => x.Step)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Completed)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired();

            // Value Object: TimeTracking
            builder.OwnsOne(x => x.TimeTracking, timeTracking =>
            {
                timeTracking.Property(t => t.TotalMinutes)
                    .HasColumnName("TimeTrackingTotalMinutes")
                    .IsRequired();

                timeTracking.Property(t => t.StartedAt)
                    .HasColumnName("TimeTrackingStartedAt")
                    .IsRequired();

                timeTracking.Property(t => t.CompletedAt)
                    .HasColumnName("TimeTrackingCompletedAt");
            });

            // Value Object: InitialAssessment (AssessmentResult) - stored as JSON
            builder.OwnsOne(x => x.InitialAssessment, assessment =>
            {
                assessment.Property(a => a.Scores)
                    .HasColumnName("AssessmentScores")
                    .HasColumnType("jsonb");

                assessment.Property(a => a.StrengthAreas)
                    .HasColumnName("AssessmentStrengths")
                    .HasColumnType("jsonb");

                assessment.Property(a => a.ImprovementAreas)
                    .HasColumnName("AssessmentImprovements")
                    .HasColumnType("jsonb");

                assessment.Property(a => a.OverallNotes)
                    .HasColumnName("AssessmentNotes")
                    .HasMaxLength(2000);
            });

            // Collection: PracticePlan (list of value objects) - stored as JSON
            builder.Property(x => x.PracticePlan)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<PracticePlanItem>>(v, (System.Text.Json.JsonSerializerOptions)null))
                .HasColumnName("PracticePlan")
                .HasColumnType("jsonb");

            // ReviewFeedback - stored as JSON
            builder.Property(x => x.ReviewFeedback)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, int>>(v, (System.Text.Json.JsonSerializerOptions)null))
                .HasColumnName("ReviewFeedback")
                .HasColumnType("jsonb");

            // MasteryValidation - stored as JSON
            builder.Property(x => x.MasteryValidation)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(v, (System.Text.Json.JsonSerializerOptions)null))
                .HasColumnName("MasteryValidation")
                .HasColumnType("jsonb");

            // Owned entities: PracticeLogs
            builder.OwnsMany(x => x.PracticeLogs, practiceLogs =>
            {
                practiceLogs.ToTable(
                    SkillDevelopmentDbProperties.DbTablePrefix + "PracticeLogs",
                    SkillDevelopmentDbProperties.DbSchema);

                practiceLogs.WithOwner().HasForeignKey("MilestoneSessionId", "MilestoneStep");

                practiceLogs.Property<System.Guid>("MilestoneSessionId");
                practiceLogs.Property<string>("MilestoneStep");

                practiceLogs.HasKey("MilestoneSessionId", "MilestoneStep", "PracticeLogId");

                // Configure PracticeLog properties
                practiceLogs.Property(p => p.Date).IsRequired();
                practiceLogs.Property(p => p.DurationMinutes).IsRequired();
                practiceLogs.Property(p => p.FocusArea).HasMaxLength(PracticeLogConsts.MaxFocusAreaLength);
                practiceLogs.Property(p => p.Notes).HasMaxLength(PracticeLogConsts.MaxNotesLength);

                practiceLogs.Property(p => p.ChallengesFaced)
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions)null))
                    .HasColumnName("ChallengesFaced")
                    .HasColumnType("jsonb");
            });

            // Owned entities: Applications
            builder.OwnsMany(x => x.Applications, applications =>
            {
                applications.ToTable(
                    SkillDevelopmentDbProperties.DbTablePrefix + "ApplicationExamples",
                    SkillDevelopmentDbProperties.DbSchema);

                applications.WithOwner().HasForeignKey("MilestoneSessionId", "MilestoneStep");

                applications.Property<System.Guid>("MilestoneSessionId");
                applications.Property<string>("MilestoneStep");

                applications.HasKey("MilestoneSessionId", "MilestoneStep", "ExampleId");

                // Configure ApplicationExample properties
                applications.Property(a => a.Description)
                    .IsRequired()
                    .HasMaxLength(ApplicationExampleConsts.MaxDescriptionLength);

                applications.Property(a => a.CompletedAt).IsRequired();

                applications.Property(a => a.Outcomes)
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions)null))
                    .HasColumnName("Outcomes")
                    .HasColumnType("jsonb");
            });
        }
    }
}
