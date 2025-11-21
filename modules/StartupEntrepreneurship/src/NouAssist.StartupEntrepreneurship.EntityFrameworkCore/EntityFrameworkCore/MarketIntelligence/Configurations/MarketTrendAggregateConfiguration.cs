using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Aggregates;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.EntityFrameworkCore.Configurations
{
    public class MarketTrendAggregateConfiguration : IEntityTypeConfiguration<MarketTrend>
    {
        public void Configure(EntityTypeBuilder<MarketTrend> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(MarketIntelligenceDbProperties.DbTablePrefix + "MarketTrends", 
                            MarketIntelligenceDbProperties.DbSchema);

            // Category Enum
            builder.Property(x => x.Category)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.Tags)
                .HasMaxLength(MarketIntelligenceConsts.Validation.MaxTagsLength);

            // TrendDescription Value Object
            builder.OwnsOne(x => x.TrendDescription, trenderBuilder =>
            {
                trenderBuilder.Property(t => t.Title)
                    .HasColumnName("Title")
                    .IsRequired()
                    .HasMaxLength(MarketIntelligenceConsts.MarketTrend.MaxTitleLength);

                trenderBuilder.Property(t => t.Description)
                    .HasColumnName("Description")
                    .IsRequired()
                    .HasMaxLength(MarketIntelligenceConsts.MarketTrend.MaxDescriptionLength);

                trenderBuilder.Property(t => t.Keywords)
                    .HasColumnName("Keywords")
                    .HasMaxLength(MarketIntelligenceConsts.MarketTrend.MaxKeywordsLength);

                trenderBuilder.Property(t => t.SourceUrl)
                    .HasColumnName("SourceUrl")
                    .HasMaxLength(MarketIntelligenceConsts.MarketTrend.MaxSourceUrlLength);
            });

            // ImpactAssessment Value Object
            builder.OwnsOne(x => x.ImpactAssessment, impactBuilder =>
            {
                impactBuilder.Property(i => i.Level)
                    .HasColumnName("ImpactLevel")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                impactBuilder.Property(i => i.Description)
                    .HasColumnName("ImpactDescription")
                    .IsRequired()
                    .HasMaxLength(MarketIntelligenceConsts.MarketTrend.MaxImpactDescriptionLength);

                impactBuilder.Property(i => i.ConfidenceScore)
                    .HasColumnName("ConfidenceScore")
                    .IsRequired()
                    .HasPrecision(4, 2);

                impactBuilder.Property(i => i.RecommendedActions)
                    .HasColumnName("RecommendedActions")
                    .HasMaxLength(MarketIntelligenceConsts.SwotAnalysis.MaxActionItemLength);
            });

            // Relationship Mapping
            builder.HasMany(x => x.DataSources)
                   .WithOne()
                   .HasForeignKey("MarketTrendId")
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => new { x.IsActive, x.Category });
            builder.HasIndex("TrendDescription_Title");
            builder.HasIndex("ImpactAssessment_Level");
        }
    }
}
