using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for KpiThreshold entity
    /// </summary>
    public class KpiThresholdConfiguration : IEntityTypeConfiguration<KpiThreshold>
    {
        public void Configure(EntityTypeBuilder<KpiThreshold> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                AnalyticsReportingDbProperties.DbTablePrefix + "KpiThresholds",
                AnalyticsReportingDbProperties.DbSchema
            );

            // Primary key is handled by Entity<TKey>

            builder.Property(x => x.Value)
                .IsRequired();

            builder.Property(x => x.Operator)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>();

            // Complex type configuration
            builder.OwnsOne(x => x.AlertCondition, alertBuilder =>
            {
                alertBuilder.Property(a => a.Expression).HasColumnName("AlertExpression")
                    .HasMaxLength(AnalyticsReportingConsts.MaxAlertConditionLogicLength);
                alertBuilder.Property(a => a.Description).HasColumnName("AlertDescription")
                    .HasMaxLength(AnalyticsReportingConsts.MaxDescriptionLength);
                alertBuilder.Property(a => a.RequireConsecutiveOccurrences).HasColumnName("RequireConsecutiveOccurrences");
                alertBuilder.Property(a => a.ConsecutiveOccurrencesRequired).HasColumnName("ConsecutiveOccurrencesRequired");
            });

            // Array property
            builder.Property(x => x.NotificationRecipients)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                );

            // Indexes
            builder.HasIndex(x => x.KpiDefinitionId);
            builder.HasIndex(x => x.Status);
        }
    }
}
