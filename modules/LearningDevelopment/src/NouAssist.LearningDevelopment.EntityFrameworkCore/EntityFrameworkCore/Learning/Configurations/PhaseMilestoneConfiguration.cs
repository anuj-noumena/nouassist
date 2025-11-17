using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.LearningDevelopment.Learning.Entities;
using NouAssist.LearningDevelopment.Learning.ValueObjects;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore.Learning.Configurations
{
    public class PhaseMilestoneConfiguration : IEntityTypeConfiguration<PhaseMilestone>
    {
        public void Configure(EntityTypeBuilder<PhaseMilestone> builder)
        {
            // Table Mapping
            builder.ToTable(
                LearningDbProperties.DbTablePrefix + "PhaseMilestones",
                LearningDbProperties.DbSchema);

            // Composite Primary Key
            builder.HasKey(x => new { x.SessionId, x.Phase });

            // Properties
            builder.Property(x => x.SessionId)
                .IsRequired()
                .HasColumnName(nameof(PhaseMilestone.SessionId));

            builder.Property(x => x.Phase)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasColumnName(nameof(PhaseMilestone.Phase));

            builder.Property(x => x.Completed)
                .IsRequired()
                .HasColumnName(nameof(PhaseMilestone.Completed));

            builder.Property(x => x.CompletedAt)
                .HasColumnName(nameof(PhaseMilestone.CompletedAt));

            builder.Property(x => x.TimeSpentMs)
                .HasColumnName(nameof(PhaseMilestone.TimeSpentMs));

            builder.Property(x => x.UpdatedAt)
                .IsRequired()
                .HasColumnName(nameof(PhaseMilestone.UpdatedAt));

            // Phase-specific data (stored as JSON for flexibility)
            builder.Property("_questions")
                .HasColumnName("Questions")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.List<string>>(v, (System.Text.Json.JsonSerializerOptions)null));

            builder.Property("_sectionReflections")
                .HasColumnName("SectionReflections")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, string>>(v, (System.Text.Json.JsonSerializerOptions)null));

            builder.Property(x => x.CornellSummary)
                .HasMaxLength(4000)
                .HasColumnName(nameof(PhaseMilestone.CornellSummary));

            // Value Object: BloomAssessmentResult (stored as JSON)
            builder.OwnsOne(x => x.BloomAssessment, bloomBuilder =>
            {
                bloomBuilder.ToJson("BloomAssessment"); // EF Core 7+ JSON columns
                
                bloomBuilder.Property(b => b.RememberScore).HasColumnName("RememberScore");
                bloomBuilder.Property(b => b.UnderstandScore).HasColumnName("UnderstandScore");
                bloomBuilder.Property(b => b.ApplyScore).HasColumnName("ApplyScore");
                bloomBuilder.Property(b => b.AnalyzeScore).HasColumnName("AnalyzeScore");
                bloomBuilder.Property(b => b.EvaluateScore).HasColumnName("EvaluateScore");
                bloomBuilder.Property(b => b.CreateScore).HasColumnName("CreateScore");
                bloomBuilder.Property(b => b.OverallScore).HasColumnName("OverallScore");
            });

            // Complex Type: SpacedCycles (stored as JSON)
            builder.Property("_spacedCycles")
                .HasColumnName("SpacedCycles")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<int, CycleProgress>>(v, (System.Text.Json.JsonSerializerOptions)null));

            // Complex Type: MasteryValidation (stored as JSON)
            builder.Property("_masteryValidation")
                .HasColumnName("MasteryValidation")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, object>>(v, (System.Text.Json.JsonSerializerOptions)null));

            // Indexes
            builder.HasIndex(x => x.SessionId)
                .HasDatabaseName("IX_Learning_PhaseMilestones_SessionId");

            builder.HasIndex(x => new { x.SessionId, x.Completed })
                .HasDatabaseName("IX_Learning_PhaseMilestones_SessionId_Completed");
        }
    }
}
