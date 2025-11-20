using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Aggregates;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.EntityFrameworkCore.Configurations
{
    public class CompetitorAggregateConfiguration : IEntityTypeConfiguration<Competitor>
    {
        public void Configure(EntityTypeBuilder<Competitor> builder)
        {
            // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency stamp)
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(MarketIntelligenceDbProperties.DbTablePrefix + "Competitors", 
                            MarketIntelligenceDbProperties.DbSchema);

            // Primary Key is handled by ConfigureByConvention()

            // Property Mappings
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(MarketIntelligenceConsts.Competitor.MaxNameLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(MarketIntelligenceConsts.Competitor.MaxDescriptionLength);

            // Enum Mapping
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ThreatLevel)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Notes)
                .HasMaxLength(MarketIntelligenceConsts.Validation.MaxNotesLength);

            builder.Property(x => x.Tags)
                .HasMaxLength(MarketIntelligenceConsts.Validation.MaxTagsLength);

            builder.Property(x => x.CompetitiveScore)
                .HasPrecision(4, 2);

            // Value Object Mapping (CompetitorInfo)
            builder.OwnsOne(x => x.CompetitorInfo, infoBuilder =>
            {
                infoBuilder.Property(c => c.Website)
                    .HasColumnName("Website")
                    .HasMaxLength(MarketIntelligenceConsts.Competitor.MaxWebsiteLength);

                infoBuilder.Property(c => c.Industry)
                    .HasColumnName("Industry")
                    .HasMaxLength(MarketIntelligenceConsts.Competitor.MaxIndustryLength);

                infoBuilder.Property(c => c.Location)
                    .HasColumnName("Location")
                    .HasMaxLength(MarketIntelligenceConsts.Competitor.MaxLocationLength);

                infoBuilder.Property(c => c.EmployeeCount)
                    .HasColumnName("EmployeeCount");

                infoBuilder.Property(c => c.FundingAmount)
                    .HasColumnName("FundingAmount")
                    .HasPrecision(18, 2);

                infoBuilder.Property(c => c.FundingSource)
                    .HasColumnName("FundingSource")
                    .HasMaxLength(MarketIntelligenceConsts.Competitor.MaxFundingSourceLength);
            });

            // Relationship Mapping (One-to-Many for child entities)
            builder.HasMany(x => x.UpdateLogs)
                   .WithOne()
                   .HasForeignKey("CompetitorId")
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasIndex(x => new { x.Status, x.ThreatLevel });
        }
    }
}
