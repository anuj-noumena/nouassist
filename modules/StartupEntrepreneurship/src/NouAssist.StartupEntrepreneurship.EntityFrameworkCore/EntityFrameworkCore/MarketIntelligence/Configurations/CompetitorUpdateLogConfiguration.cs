using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.EntityFrameworkCore.Configurations
{
    public class CompetitorUpdateLogConfiguration : IEntityTypeConfiguration<CompetitorUpdateLog>
    {
        public void Configure(EntityTypeBuilder<CompetitorUpdateLog> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(MarketIntelligenceDbProperties.DbTablePrefix + "CompetitorUpdateLogs", 
                            MarketIntelligenceDbProperties.DbSchema);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(MarketIntelligenceConsts.MarketTrend.MaxTitleLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(MarketIntelligenceConsts.MarketTrend.MaxDescriptionLength);

            builder.Property(x => x.Source)
                .HasMaxLength(MarketIntelligenceConsts.MarketTrend.MaxSourceUrlLength);

            builder.Property(x => x.Tags)
                .HasMaxLength(MarketIntelligenceConsts.Validation.MaxTagsLength);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ImpactLevel)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Indexing
            builder.HasIndex(x => x.CompetitorId);
            builder.HasIndex(x => x.UpdateDate);
        }
    }
}
