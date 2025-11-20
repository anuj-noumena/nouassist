using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Aggregates;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.EntityFrameworkCore.Configurations
{
    public class CustomerSegmentProfileAggregateConfiguration : IEntityTypeConfiguration<CustomerSegmentProfile>
    {
        public void Configure(EntityTypeBuilder<CustomerSegmentProfile> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(MarketIntelligenceDbProperties.DbTablePrefix + "CustomerSegmentProfiles", 
                            MarketIntelligenceDbProperties.DbSchema);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(MarketIntelligenceConsts.CustomerSegment.MaxNameLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(MarketIntelligenceConsts.CustomerSegment.MaxDescriptionLength);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.PrimaryNeeds)
                .IsRequired()
                .HasMaxLength(MarketIntelligenceConsts.CustomerSegment.MaxNeedDescriptionLength);

            builder.Property(x => x.PainPoints)
                .HasMaxLength(MarketIntelligenceConsts.CustomerSegment.MaxNeedDescriptionLength);

            builder.Property(x => x.PreferredChannels)
                .HasMaxLength(MarketIntelligenceConsts.CustomerSegment.MaxNeedDescriptionLength);

            builder.Property(x => x.EstimatedSize)
                .HasPrecision(18, 2);

            builder.Property(x => x.EstimatedValue)
                .HasPrecision(18, 2);

            builder.Property(x => x.IsActive)
                .IsRequired();

            // Demographics Value Object
            builder.OwnsOne(x => x.Demographics, demoBuilder =>
            {
                demoBuilder.Property(d => d.AgeRange)
                    .HasColumnName("DemographicsAgeRange")
                    .HasMaxLength(MarketIntelligenceConsts.CustomerSegment.MaxDemographicValueLength);

                demoBuilder.Property(d => d.Gender)
                    .HasColumnName("DemographicsGender")
                    .HasMaxLength(MarketIntelligenceConsts.CustomerSegment.MaxDemographicValueLength);

                demoBuilder.Property(d => d.Income)
                    .HasColumnName("DemographicsIncome")
                    .HasMaxLength(MarketIntelligenceConsts.CustomerSegment.MaxDemographicValueLength);

                demoBuilder.Property(d => d.Education)
                    .HasColumnName("DemographicsEducation")
                    .HasMaxLength(MarketIntelligenceConsts.CustomerSegment.MaxDemographicValueLength);

                demoBuilder.Property(d => d.Location)
                    .HasColumnName("DemographicsLocation")
                    .HasMaxLength(MarketIntelligenceConsts.CustomerSegment.MaxDemographicValueLength);

                demoBuilder.Property(d => d.Occupation)
                    .HasColumnName("DemographicsOccupation")
                    .HasMaxLength(MarketIntelligenceConsts.CustomerSegment.MaxDemographicValueLength);
            });

            // Psychographics Value Object
            builder.OwnsOne(x => x.Psychographics, psychoBuilder =>
            {
                psychoBuilder.Property(p => p.Interests)
                    .HasColumnName("PsychographicsInterests")
                    .HasMaxLength(MarketIntelligenceConsts.CustomerSegment.MaxPsychographicValueLength);

                psychoBuilder.Property(p => p.Values)
                    .HasColumnName("PsychographicsValues")
                    .HasMaxLength(MarketIntelligenceConsts.CustomerSegment.MaxPsychographicValueLength);

                psychoBuilder.Property(p => p.Lifestyle)
                    .HasColumnName("PsychographicsLifestyle")
                    .HasMaxLength(MarketIntelligenceConsts.CustomerSegment.MaxPsychographicValueLength);

                psychoBuilder.Property(p => p.Personality)
                    .HasColumnName("PsychographicsPersonality")
                    .HasMaxLength(MarketIntelligenceConsts.CustomerSegment.MaxPsychographicValueLength);

                psychoBuilder.Property(p => p.Attitudes)
                    .HasColumnName("PsychographicsAttitudes")
                    .HasMaxLength(MarketIntelligenceConsts.CustomerSegment.MaxPsychographicValueLength);
            });

            // Relationship Mapping
            builder.HasMany(x => x.Characteristics)
                   .WithOne()
                   .HasForeignKey("CustomerSegmentProfileId")
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasIndex(x => new { x.Type, x.IsActive });
        }
    }
}
