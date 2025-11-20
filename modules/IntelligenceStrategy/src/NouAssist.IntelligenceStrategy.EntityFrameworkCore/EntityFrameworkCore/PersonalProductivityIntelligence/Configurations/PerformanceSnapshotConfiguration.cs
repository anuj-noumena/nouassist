using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Constants;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.EntityFrameworkCore.Configurations;

/// <summary>
/// Configuration for the PerformanceSnapshot aggregate
/// </summary>
public class PerformanceSnapshotConfiguration : IEntityTypeConfiguration<PerformanceSnapshot>
{
    public void Configure(EntityTypeBuilder<PerformanceSnapshot> builder)
    {
        // Apply ABP base conventions
        builder.ConfigureByConvention();

        // Table mapping
        builder.ToTable(
            PersonalProductivityIntelligenceDbProperties.DbTablePrefix + "PerformanceSnapshots",
            PersonalProductivityIntelligenceDbProperties.DbSchema);

        // Primary key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Title).IsRequired().HasMaxLength(PersonalProductivityIntelligenceConsts.MaxNameLength);
        builder.Property(x => x.SnapshotType).IsRequired().HasMaxLength(50);
        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.EndDate).IsRequired();
        builder.Property(x => x.Summary).HasMaxLength(PersonalProductivityIntelligenceConsts.MaxDescriptionLength);

        // PerformanceMetricValue relationship (one-to-many)
        builder.OwnsMany(x => x.MetricValues, metricBuilder =>
        {
            metricBuilder.WithOwner().HasForeignKey("PerformanceSnapshotId");
            metricBuilder.HasKey("PerformanceSnapshotId", "MetricName");
            
            metricBuilder.Property(m => m.MetricName).HasMaxLength(128).IsRequired();
            metricBuilder.Property(m => m.Value).IsRequired();
            metricBuilder.Property(m => m.Unit).HasMaxLength(50);
            metricBuilder.Property(m => m.Category).HasMaxLength(50);
            
            metricBuilder.ToTable(PersonalProductivityIntelligenceDbProperties.DbTablePrefix + "PerformanceMetricValues", PersonalProductivityIntelligenceDbProperties.DbSchema);
        });

        // BenchmarkData relationship (one-to-many)
        builder.OwnsMany(x => x.Benchmarks, benchmarkBuilder =>
        {
            benchmarkBuilder.WithOwner().HasForeignKey("PerformanceSnapshotId");
            benchmarkBuilder.HasKey("PerformanceSnapshotId", "BenchmarkName", "BenchmarkType");
            
            benchmarkBuilder.Property(b => b.BenchmarkName).HasMaxLength(128).IsRequired();
            benchmarkBuilder.Property(b => b.BenchmarkType).HasMaxLength(50).IsRequired();
            benchmarkBuilder.Property(b => b.Value).IsRequired();
            benchmarkBuilder.Property(b => b.Unit).HasMaxLength(50);
            
            benchmarkBuilder.ToTable(PersonalProductivityIntelligenceDbProperties.DbTablePrefix + "BenchmarkData", PersonalProductivityIntelligenceDbProperties.DbSchema);
        });

        // InsightText relationship (one-to-many)
        builder.OwnsMany(x => x.Insights, insightBuilder =>
        {
            insightBuilder.WithOwner().HasForeignKey("PerformanceSnapshotId");
            insightBuilder.HasKey("PerformanceSnapshotId", "Text");
            
            insightBuilder.Property(i => i.Text).HasMaxLength(PersonalProductivityIntelligenceConsts.MaxInsightTextLength).IsRequired();
            insightBuilder.Property(i => i.Category).HasMaxLength(50);
            insightBuilder.Property(i => i.RelevanceScore);
            
            insightBuilder.ToTable(PersonalProductivityIntelligenceDbProperties.DbTablePrefix + "InsightTexts", PersonalProductivityIntelligenceDbProperties.DbSchema);
        });

        // Indexing
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.SnapshotType);
        builder.HasIndex(x => x.StartDate);
        builder.HasIndex(x => x.EndDate);
        builder.HasIndex(x => new { x.UserId, x.SnapshotType, x.StartDate });
    }
}
