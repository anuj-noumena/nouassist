using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.EntityFrameworkCore.Configurations
{
    public class GrowthExperimentConfiguration : IEntityTypeConfiguration<GrowthExperiment>
    {
        public void Configure(EntityTypeBuilder<GrowthExperiment> builder)
        {
            // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency stamp)
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                GrowthExperimentationDbProperties.DbTablePrefix + "Experiments", 
                GrowthExperimentationDbProperties.DbSchema);

            // Primary Key (handled by Entity<TKey> / AggregateRoot<TKey>)
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(GrowthExperimentationConstants.Experiments.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(GrowthExperimentationConstants.Experiments.MaxDescriptionLength);

            // Enum Mapping
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Value Object Mapping - ExperimentGoal
            builder.OwnsOne(x => x.Goal, goalBuilder => 
            {
                goalBuilder.Property(g => g.Description)
                    .HasColumnName("GoalDescription")
                    .HasMaxLength(GrowthExperimentationConstants.Experiments.MaxDescriptionLength);
                    
                goalBuilder.Property(g => g.ExpectedImprovement)
                    .HasColumnName("GoalExpectedImprovement");
                    
                goalBuilder.Property(g => g.MinimumViableImprovement)
                    .HasColumnName("GoalMinimumViableImprovement");
            });

            // Complex collection of value objects - TargetMetrics
            builder.OwnsMany(x => x.TargetMetrics, metricsBuilder => 
            {
                metricsBuilder.ToTable(
                    GrowthExperimentationDbProperties.DbTablePrefix + "ExperimentTargetMetrics", 
                    GrowthExperimentationDbProperties.DbSchema);
                    
                metricsBuilder.WithOwner().HasForeignKey("GrowthExperimentId");
                metricsBuilder.Property(m => m.Name).HasMaxLength(100).IsRequired();
                metricsBuilder.Property(m => m.Type).HasConversion<string>().HasMaxLength(50);
                metricsBuilder.Property(m => m.BaselineValue);
                metricsBuilder.Property(m => m.TargetValue);
                metricsBuilder.Property(m => m.IsPrimary);
            });

            // Relationship Mapping - Hypotheses
            builder.HasMany(x => x.Hypotheses)
                .WithOne()
                .HasForeignKey(h => h.ExperimentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship Mapping - Variants
            builder.HasMany(x => x.Variants)
                .WithOne()
                .HasForeignKey(v => v.ExperimentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Value Object Mapping - StatisticalSignificance
            builder.OwnsOne(x => x.Significance, sigBuilder => 
            {
                sigBuilder.Property(s => s.PValue).HasColumnName("SignificancePValue");
                sigBuilder.Property(s => s.ConfidenceLevel).HasColumnName("ConfidenceLevel");
                sigBuilder.Property(s => s.SampleSize).HasColumnName("SignificanceSampleSize");
                sigBuilder.Property(s => s.Power).HasColumnName("StatisticalPower");
            });

            // Value Object Mapping - ExperimentLearnings
            builder.OwnsOne(x => x.Learnings, learningsBuilder => 
            {
                learningsBuilder.Property(l => l.Insights)
                    .HasColumnName("LearningsInsights")
                    .HasMaxLength(GrowthExperimentationConstants.Experiments.MaxLearningsTextLength);
                    
                learningsBuilder.Property(l => l.UnexpectedOutcomes)
                    .HasColumnName("LearningsUnexpectedOutcomes")
                    .HasMaxLength(GrowthExperimentationConstants.Experiments.MaxLearningsTextLength);
                    
                learningsBuilder.Property(l => l.Recommendations)
                    .HasColumnName("LearningsRecommendations")
                    .HasMaxLength(GrowthExperimentationConstants.Experiments.MaxLearningsTextLength);
                    
                learningsBuilder.Property(l => l.DocumentedBy)
                    .HasColumnName("LearningsDocumentedBy");
                    
                learningsBuilder.Property(l => l.DocumentedAt)
                    .HasColumnName("LearningsDocumentedAt");
            });

            // Indexing
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.Status);
        }
    }
}
