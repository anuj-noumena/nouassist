using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.LearningDevelopment.SkillDevelopment.Aggregates;
using NouAssist.SkillDevelopment;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore.SkillDevelopment.Configurations
{
    public class SkillConfiguration : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                SkillDevelopmentDbProperties.DbTablePrefix + "Skills",
                SkillDevelopmentDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(SkillConsts.MaxTitleLength);

            builder.Property(x => x.Description)
                .HasMaxLength(SkillConsts.MaxDescriptionLength);

            builder.Property(x => x.Category)
                .HasMaxLength(SkillConsts.MaxCategoryLength);

            builder.Property(x => x.DevelopmentGoals)
                .HasMaxLength(SkillConsts.MaxDevelopmentGoalsLength);

            builder.Property(x => x.IsCategory)
                .IsRequired();

            builder.Property(x => x.TargetProficiency)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.EstimatedPracticeHours)
                .IsRequired();

            // Value Object: CurrentProficiency (ProficiencyScore)
            builder.OwnsOne(x => x.CurrentProficiency, proficiency =>
            {
                proficiency.Property(p => p.Score)
                    .HasColumnName("CurrentProficiencyScore")
                    .IsRequired();

                proficiency.Property(p => p.AssessedAt)
                    .HasColumnName("CurrentProficiencyAssessedAt")
                    .IsRequired();

                proficiency.Property(p => p.AssessmentNotes)
                    .HasColumnName("CurrentProficiencyNotes")
                    .HasMaxLength(1000);
            });

            // Collections stored as JSON
            builder.Property(x => x.Prerequisites)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.List<Guid>>(v, (System.Text.Json.JsonSerializerOptions)null))
                .HasColumnName("Prerequisites")
                .HasColumnType("jsonb");

            builder.Property(x => x.Resources)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.List<string>>(v, (System.Text.Json.JsonSerializerOptions)null))
                .HasColumnName("Resources")
                .HasColumnType("jsonb");

            builder.Property(x => x.SubSkillIds)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.List<Guid>>(v, (System.Text.Json.JsonSerializerOptions)null))
                .HasColumnName("SubSkillIds")
                .HasColumnType("jsonb");

            // Indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.ParentId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.Category);
            builder.HasIndex(x => new { x.UserId, x.Status });

            // Extra Properties (ABP handles JSON mapping by default)
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(Skill.ExtraProperties));

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
