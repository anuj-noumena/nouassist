using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.EntityFrameworkCore.Configurations
{
    public class TrendDataSourceConfiguration : IEntityTypeConfiguration<TrendDataSource>
    {
        public void Configure(EntityTypeBuilder<TrendDataSource> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(MarketIntelligenceDbProperties.DbTablePrefix + "TrendDataSources", 
                            MarketIntelligenceDbProperties.DbSchema);

            builder.Property(x => x.SourceName)
                .IsRequired()
                .HasMaxLength(MarketIntelligenceConsts.MarketTrend.MaxTitleLength);

            builder.Property(x => x.SourceUrl)
                .IsRequired()
                .HasMaxLength(MarketIntelligenceConsts.MarketTrend.MaxSourceUrlLength);

            builder.Property(x => x.SourceType)
                .HasMaxLength(100);

            builder.Property(x => x.Reliability)
                .IsRequired()
                .HasPrecision(4, 2);

            builder.Property(x => x.DataQuality)
                .HasMaxLength(200);

            // Indexing
            builder.HasIndex(x => x.MarketTrendId);
            builder.HasIndex(x => x.Reliability);
        }
    }
}
