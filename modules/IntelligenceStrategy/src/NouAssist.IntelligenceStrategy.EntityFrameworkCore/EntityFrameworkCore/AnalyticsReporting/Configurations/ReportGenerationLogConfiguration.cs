using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for ReportGenerationLog entity
    /// </summary>
    public class ReportGenerationLogConfiguration : IEntityTypeConfiguration<ReportGenerationLog>
    {
        public void Configure(EntityTypeBuilder<ReportGenerationLog> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                AnalyticsReportingDbProperties.DbTablePrefix + "ReportGenerationLogs",
                AnalyticsReportingDbProperties.DbSchema
            );

            // Primary key is handled by Entity<TKey>

            builder.Property(x => x.StartTime)
                .IsRequired();

            builder.Property(x => x.EndTime);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.ErrorMessage)
                .HasMaxLength(AnalyticsReportingConsts.MaxDescriptionLength);

            builder.Property(x => x.ResultPath)
                .HasMaxLength(1000);

            builder.Property(x => x.Format)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.ParametersJson);

            // Indexes
            builder.HasIndex(x => x.ReportConfigurationId);
            builder.HasIndex(x => x.ReportScheduleId);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.StartTime);
            builder.HasIndex(x => x.Status);
        }
    }
}
