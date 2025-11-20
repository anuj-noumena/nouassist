using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for KpiAlertLog entity
    /// </summary>
    public class KpiAlertLogConfiguration : IEntityTypeConfiguration<KpiAlertLog>
    {
        public void Configure(EntityTypeBuilder<KpiAlertLog> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                AnalyticsReportingDbProperties.DbTablePrefix + "KpiAlertLogs",
                AnalyticsReportingDbProperties.DbSchema
            );

            // Primary key is handled by Entity<TKey>

            builder.Property(x => x.Timestamp)
                .IsRequired();

            builder.Property(x => x.Value)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.NotificationSent)
                .IsRequired();

            builder.Property(x => x.NotificationSentAt);

            builder.Property(x => x.Message)
                .HasMaxLength(AnalyticsReportingConsts.MaxDescriptionLength);

            // Indexes
            builder.HasIndex(x => x.KpiDefinitionId);
            builder.HasIndex(x => x.KpiThresholdId);
            builder.HasIndex(x => x.Timestamp);
            builder.HasIndex(x => x.Status);
        }
    }
}
