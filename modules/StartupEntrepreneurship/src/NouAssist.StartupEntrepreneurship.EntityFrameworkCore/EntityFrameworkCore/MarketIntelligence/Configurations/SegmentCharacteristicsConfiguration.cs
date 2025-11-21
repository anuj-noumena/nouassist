using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.EntityFrameworkCore.Configurations
{
    public class SegmentCharacteristicsConfiguration : IEntityTypeConfiguration<SegmentCharacteristics>
    {
        public void Configure(EntityTypeBuilder<SegmentCharacteristics> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(MarketIntelligenceDbProperties.DbTablePrefix + "SegmentCharacteristics", 
                            MarketIntelligenceDbProperties.DbSchema);

            builder.Property(x => x.CharacteristicName)
                .IsRequired()
                .HasMaxLength(MarketIntelligenceConsts.CustomerSegment.MaxCharacteristicNameLength);

            builder.Property(x => x.CharacteristicValue)
                .IsRequired()
                .HasMaxLength(MarketIntelligenceConsts.CustomerSegment.MaxCharacteristicValueLength);

            builder.Property(x => x.Category)
                .HasMaxLength(100);

            builder.Property(x => x.Importance)
                .IsRequired()
                .HasPrecision(4, 2);

            builder.Property(x => x.Notes)
                .HasMaxLength(MarketIntelligenceConsts.Validation.MaxNotesLength);

            // Indexing
            builder.HasIndex(x => x.CustomerSegmentProfileId);
            builder.HasIndex(x => new { x.CustomerSegmentProfileId, x.Category });
        }
    }
}
