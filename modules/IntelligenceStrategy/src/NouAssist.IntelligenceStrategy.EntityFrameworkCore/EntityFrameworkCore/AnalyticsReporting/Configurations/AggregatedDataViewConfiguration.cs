using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for AggregatedDataView entity
    /// </summary>
    public class AggregatedDataViewConfiguration : IEntityTypeConfiguration<AggregatedDataView>
    {
        public void Configure(EntityTypeBuilder<AggregatedDataView> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                AnalyticsReportingDbProperties.DbTablePrefix + "AggregatedDataViews",
                AnalyticsReportingDbProperties.DbSchema
            );

            // Primary key is handled by Entity<TKey>

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(AnalyticsReportingConsts.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(AnalyticsReportingConsts.MaxDescriptionLength);

            builder.Property(x => x.LastRefreshed)
                .IsRequired();

            builder.Property(x => x.AggregationDefinition)
                .IsRequired();

            builder.Property(x => x.ValidationStatus)
                .IsRequired()
                .HasConversion<string>();

            // Complex type configuration
            builder.OwnsOne(x => x.RefreshInterval, refreshBuilder =>
            {
                refreshBuilder.Property(r => r.Value).HasColumnName("RefreshIntervalValue");
                refreshBuilder.Property(r => r.Unit).HasColumnName("RefreshIntervalUnit").HasMaxLength(20);
                refreshBuilder.Property(r => r.AutoRefreshEnabled).HasColumnName("AutoRefreshEnabled");
            });

            // Array property
            builder.Property(x => x.SourceDataSourceIds)
                .HasConversion(
                    v => string.Join(',', v.Select(id => id.ToString())),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(id => Guid.Parse(id))
                        .ToArray()
                );

            // Indexes
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.ValidationStatus);
        }
    }
}
