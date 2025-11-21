using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Aggregates;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.EntityFrameworkCore.Configurations
{
    public class SwotAnalysisAggregateConfiguration : IEntityTypeConfiguration<SwotAnalysis>
    {
        public void Configure(EntityTypeBuilder<SwotAnalysis> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(MarketIntelligenceDbProperties.DbTablePrefix + "SwotAnalyses", 
                            MarketIntelligenceDbProperties.DbSchema);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(MarketIntelligenceConsts.SwotAnalysis.MaxTitleLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(MarketIntelligenceConsts.SwotAnalysis.MaxDescriptionLength);

            builder.Property(x => x.StrategicRecommendation)
                .HasMaxLength(MarketIntelligenceConsts.SwotAnalysis.MaxDescriptionLength);

            builder.Property(x => x.AnalysisNotes)
                .HasMaxLength(MarketIntelligenceConsts.Validation.MaxNotesLength);

            builder.Property(x => x.IsCompleted)
                .IsRequired();

            // Relationship Mapping
            builder.HasMany(x => x.Items)
                   .WithOne()
                   .HasForeignKey("SwotAnalysisId")
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.Title);
            builder.HasIndex(x => x.IsCompleted);
            builder.HasIndex(x => x.AnalysisDate);
        }
    }
}
