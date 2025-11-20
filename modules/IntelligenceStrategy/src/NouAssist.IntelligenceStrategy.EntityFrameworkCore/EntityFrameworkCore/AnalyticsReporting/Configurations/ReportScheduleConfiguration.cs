using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for ReportSchedule entity
    /// </summary>
    public class ReportScheduleConfiguration : IEntityTypeConfiguration<ReportSchedule>
    {
        public void Configure(EntityTypeBuilder<ReportSchedule> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                AnalyticsReportingDbProperties.DbTablePrefix + "ReportSchedules",
                AnalyticsReportingDbProperties.DbSchema
            );

            // Primary key is handled by Entity<TKey>

            builder.Property(x => x.Format)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.EmailSubject)
                .HasMaxLength(AnalyticsReportingConsts.MaxNameLength);

            builder.Property(x => x.EmailBody)
                .HasMaxLength(AnalyticsReportingConsts.MaxDescriptionLength);

            builder.Property(x => x.LastExecuted);
            
            builder.Property(x => x.NextScheduled);

            // Complex type configuration
            builder.OwnsOne(x => x.Timing, timingBuilder =>
            {
                timingBuilder.Property(t => t.CronExpression).HasColumnName("CronExpression").HasMaxLength(100);
                timingBuilder.Property(t => t.TimeZone).HasColumnName("TimeZone").HasMaxLength(50);
                timingBuilder.Property(t => t.StartDate).HasColumnName("ScheduleStartDate");
                timingBuilder.Property(t => t.EndDate).HasColumnName("ScheduleEndDate");
                timingBuilder.Property(t => t.IsActive).HasColumnName("IsScheduleActive");
            });

            // Array property
            builder.Property(x => x.Recipients)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                );

            // Indexes
            builder.HasIndex(x => x.ReportConfigurationId);
        }
    }
}
