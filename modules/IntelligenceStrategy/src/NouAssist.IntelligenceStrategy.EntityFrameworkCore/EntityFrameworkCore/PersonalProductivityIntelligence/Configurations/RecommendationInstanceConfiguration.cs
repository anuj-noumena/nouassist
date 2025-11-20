using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Constants;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.EntityFrameworkCore.Configurations;

/// <summary>
/// Configuration for the RecommendationInstance aggregate
/// </summary>
public class RecommendationInstanceConfiguration : IEntityTypeConfiguration<RecommendationInstance>
{
    public void Configure(EntityTypeBuilder<RecommendationInstance> builder)
    {
        // Apply ABP base conventions
        builder.ConfigureByConvention();

        // Table mapping
        builder.ToTable(
            PersonalProductivityIntelligenceDbProperties.DbTablePrefix + "RecommendationInstances",
            PersonalProductivityIntelligenceDbProperties.DbSchema);

        // Primary key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.RecommendationType).IsRequired().HasConversion<string>().HasMaxLength(50);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(PersonalProductivityIntelligenceConsts.MaxNameLength);
        builder.Property(x => x.Description).HasMaxLength(PersonalProductivityIntelligenceConsts.MaxDescriptionLength);
        builder.Property(x => x.GeneratedAt).IsRequired();
        builder.Property(x => x.ExpiresAt);
        builder.Property(x => x.IsViewed).IsRequired().HasDefaultValue(false);
        builder.Property(x => x.IsActedOn).IsRequired().HasDefaultValue(false);
        builder.Property(x => x.ActedOnAt);
        builder.Property(x => x.IsDismissed).IsRequired().HasDefaultValue(false);
        builder.Property(x => x.DismissedAt);
        builder.Property(x => x.UserRating);
        builder.Property(x => x.TargetEntityType).HasMaxLength(50);
        builder.Property(x => x.TargetEntityId);

        // PriorityScore value object
        builder.OwnsOne(x => x.Priority, priorityBuilder =>
        {
            priorityBuilder.Property(p => p.Value).HasColumnName("PriorityValue").IsRequired();
        });

        // RecommendationRule relationship (one-to-many)
        builder.OwnsMany(x => x.Rules, ruleBuilder =>
        {
            ruleBuilder.WithOwner().HasForeignKey("RecommendationInstanceId");
            ruleBuilder.HasKey("RecommendationInstanceId", "RuleName");
            
            ruleBuilder.Property(r => r.RuleName).HasMaxLength(128).IsRequired();
            ruleBuilder.Property(r => r.RuleDescription).HasMaxLength(512);
            ruleBuilder.Property(r => r.ContributionWeight).IsRequired();
            
            ruleBuilder.ToTable(PersonalProductivityIntelligenceDbProperties.DbTablePrefix + "RecommendationRules", PersonalProductivityIntelligenceDbProperties.DbSchema);
        });

        // ExplainabilityLog relationship (one-to-many)
        builder.OwnsMany(x => x.ExplainabilityLogs, logBuilder =>
        {
            logBuilder.WithOwner().HasForeignKey("RecommendationInstanceId");
            logBuilder.HasKey("RecommendationInstanceId", "FactorName");
            
            logBuilder.Property(l => l.FactorName).HasMaxLength(128).IsRequired();
            logBuilder.Property(l => l.Explanation).HasMaxLength(1024).IsRequired();
            logBuilder.Property(l => l.Impact).IsRequired();
            logBuilder.Property(l => l.EvidenceType).HasMaxLength(50);
            logBuilder.Property(l => l.EvidenceData).HasColumnType("jsonb");
            
            logBuilder.ToTable(PersonalProductivityIntelligenceDbProperties.DbTablePrefix + "RecommendationExplainabilityLogs", PersonalProductivityIntelligenceDbProperties.DbSchema);
        });

        // Indexing
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.RecommendationType);
        builder.HasIndex(x => x.GeneratedAt);
        builder.HasIndex(x => x.IsActedOn);
        builder.HasIndex(x => x.IsDismissed);
        builder.HasIndex(x => new { x.UserId, x.IsActedOn, x.IsDismissed });
    }
}
