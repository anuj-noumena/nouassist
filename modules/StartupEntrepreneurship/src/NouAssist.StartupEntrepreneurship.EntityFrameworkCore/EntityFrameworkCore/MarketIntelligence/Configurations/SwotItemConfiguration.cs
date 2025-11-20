using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.EntityFrameworkCore.Configurations
{
    public class SwotItemConfiguration : IEntityTypeConfiguration<SwotItem>
    {
        public void Configure(EntityTypeBuilder<SwotItem> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(MarketIntelligenceDbProperties.DbTablePrefix + "SwotItems", 
                            MarketIntelligenceDbProperties.DbSchema);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(MarketIntelligenceConsts.SwotAnalysis.MaxItemTitleLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(MarketIntelligenceConsts.SwotAnalysis.MaxItemDescriptionLength);

            builder.Property(x => x.Category)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Impact)
                .IsRequired()
                .HasPrecision(4, 2);

            builder.Property(x => x.Priority)
                .IsRequired();

            builder.Property(x => x.ActionItems)
                .HasMaxLength(MarketIntelligenceConsts.SwotAnalysis.MaxActionItemLength);

            builder.Property(x => x.Tags)
                .HasMaxLength(MarketIntelligenceConsts.Validation.MaxTagsLength);

            // Indexing
            builder.HasIndex(x => x.SwotAnalysisId);
            builder.HasIndex(x => new { x.SwotAnalysisId, x.Category });
        }
    }
}
