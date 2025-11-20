using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.EntityFrameworkCore.Configurations
{
    public class MarketingChannelPerformanceConfiguration : IEntityTypeConfiguration<MarketingChannelPerformance>
    {
        public void Configure(EntityTypeBuilder<MarketingChannelPerformance> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                GrowthExperimentationDbProperties.DbTablePrefix + "MarketingChannels", 
                GrowthExperimentationDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(GrowthExperimentationConstants.MarketingChannels.MaxChannelNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(GrowthExperimentationConstants.MarketingChannels.MaxChannelDescriptionLength);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.IsActive);

            // Value Object Mapping - ChannelDetails
            builder.OwnsOne(x => x.Details, detailsBuilder => 
            {
                detailsBuilder.Property(d => d.Platform)
                    .HasColumnName("Platform")
                    .HasMaxLength(100)
                    .IsRequired();
                    
                detailsBuilder.Property(d => d.AccountId)
                    .HasColumnName("AccountId")
                    .HasMaxLength(100);
                    
                detailsBuilder.Property(d => d.Url)
                    .HasColumnName("ChannelUrl")
                    .HasMaxLength(2000);
                    
                detailsBuilder.Property(d => d.Settings)
                    .HasColumnName("Settings")
                    .HasColumnType("jsonb");
            });

            // Value Object Mapping - CostPerAcquisition
            builder.OwnsOne(x => x.CostPerAcquisition, cpaBuilder => 
            {
                cpaBuilder.Property(c => c.Cost)
                    .HasColumnName("CpaCost")
                    .HasColumnType("decimal(18,2)");
                    
                cpaBuilder.Property(c => c.Acquisitions)
                    .HasColumnName("CpaAcquisitions");
                    
                cpaBuilder.Property(c => c.PeriodStart)
                    .HasColumnName("CpaPeriodStart");
                    
                cpaBuilder.Property(c => c.PeriodEnd)
                    .HasColumnName("CpaPeriodEnd");
            });

            // Value Object Mapping - ReturnOnAdSpend
            builder.OwnsOne(x => x.ReturnOnAdSpend, roasBuilder => 
            {
                roasBuilder.Property(r => r.AdSpend)
                    .HasColumnName("RoasAdSpend")
                    .HasColumnType("decimal(18,2)");
                    
                roasBuilder.Property(r => r.Revenue)
                    .HasColumnName("RoasRevenue")
                    .HasColumnType("decimal(18,2)");
                    
                roasBuilder.Property(r => r.PeriodStart)
                    .HasColumnName("RoasPeriodStart");
                    
                roasBuilder.Property(r => r.PeriodEnd)
                    .HasColumnName("RoasPeriodEnd");
            });

            // Relationship Mapping - Metrics
            builder.HasMany(x => x.Metrics)
                .WithOne()
                .HasForeignKey(m => m.ChannelId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasIndex(x => x.Type);
            builder.HasIndex(x => x.IsActive);
        }
    }
}
