using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Constants;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.EntityFrameworkCore.Configurations;

/// <summary>
/// Configuration for the PredictionInstance aggregate
/// </summary>
public class PredictionInstanceConfiguration : IEntityTypeConfiguration<PredictionInstance>
{
    public void Configure(EntityTypeBuilder<PredictionInstance> builder)
    {
        // Apply ABP base conventions
        builder.ConfigureByConvention();

        // Table mapping
        builder.ToTable(
            PersonalProductivityIntelligenceDbProperties.DbTablePrefix + "PredictionInstances",
            PersonalProductivityIntelligenceDbProperties.DbSchema);

        // Primary key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.PredictionType).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(PersonalProductivityIntelligenceConsts.MaxNameLength);
        builder.Property(x => x.Description).HasMaxLength(PersonalProductivityIntelligenceConsts.MaxDescriptionLength);
        builder.Property(x => x.ConfidenceLevel).IsRequired().HasConversion<string>().HasMaxLength(50);
        builder.Property(x => x.ConfidenceValue).IsRequired();
        builder.Property(x => x.PredictionTime).IsRequired();
        builder.Property(x => x.TargetTime);
        builder.Property(x => x.TargetEntityType).HasMaxLength(50);
        builder.Property(x => x.TargetEntityId);
        builder.Property(x => x.OutcomeValue);

        // TimeEstimate value object
        builder.OwnsOne(x => x.TimeEstimate, timeBuilder =>
        {
            timeBuilder.Property(t => t.MinMinutes).HasColumnName("TimeEstimateMinMinutes");
            timeBuilder.Property(t => t.MostLikelyMinutes).HasColumnName("TimeEstimateMostLikelyMinutes");
            timeBuilder.Property(t => t.MaxMinutes).HasColumnName("TimeEstimateMaxMinutes");
            timeBuilder.Ignore(t => t.ExpectedMinutes); // This is a calculated property
        });

        // RiskProbability value object
        builder.OwnsOne(x => x.RiskProbability, riskBuilder =>
        {
            riskBuilder.Property(r => r.Value).HasColumnName("RiskProbabilityValue");
        });

        // PredictionModelMetadata entity
        builder.OwnsOne(x => x.ModelMetadata, metadataBuilder =>
        {
            metadataBuilder.Property(m => m.ModelName).HasColumnName("ModelName").HasMaxLength(128);
            metadataBuilder.Property(m => m.ModelVersion).HasColumnName("ModelVersion").HasMaxLength(50);
            metadataBuilder.Property(m => m.Features).HasColumnName("ModelFeatures").HasMaxLength(1024);
            metadataBuilder.Property<string>("FeatureImportanceString")
                .HasColumnName("FeatureImportance")
                .HasColumnType("jsonb");
            
            metadataBuilder.Ignore(m => m.FeatureImportance);
        });

        // ExplainabilityLog relationship (one-to-many)
        builder.OwnsMany(x => x.ExplainabilityLogs, logBuilder =>
        {
            logBuilder.WithOwner().HasForeignKey("PredictionInstanceId");
            logBuilder.HasKey("PredictionInstanceId", "FactorName");
            
            logBuilder.Property(l => l.FactorName).HasMaxLength(128).IsRequired();
            logBuilder.Property(l => l.Explanation).HasMaxLength(1024).IsRequired();
            logBuilder.Property(l => l.Impact).IsRequired();
            logBuilder.Property(l => l.EvidenceType).HasMaxLength(50);
            logBuilder.Property(l => l.EvidenceData).HasColumnType("jsonb");
            
            logBuilder.ToTable(PersonalProductivityIntelligenceDbProperties.DbTablePrefix + "PredictionExplainabilityLogs", PersonalProductivityIntelligenceDbProperties.DbSchema);
        });

        // Indexing
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.PredictionType);
        builder.HasIndex(x => x.ConfidenceLevel);
        builder.HasIndex(x => new { x.TargetEntityType, x.TargetEntityId }).HasFilter("\"TargetEntityId\" IS NOT NULL");
    }
}
