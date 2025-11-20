using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for WidgetInstance entity
    /// </summary>
    public class WidgetInstanceConfiguration : IEntityTypeConfiguration<WidgetInstance>
    {
        public void Configure(EntityTypeBuilder<WidgetInstance> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                AnalyticsReportingDbProperties.DbTablePrefix + "WidgetInstances",
                AnalyticsReportingDbProperties.DbSchema
            );

            // Primary key is handled by Entity<TKey>

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.PositionX)
                .IsRequired();

            builder.Property(x => x.PositionY)
                .IsRequired();

            builder.Property(x => x.Width)
                .IsRequired();

            builder.Property(x => x.Height)
                .IsRequired();

            builder.Property(x => x.DataSourceId);

            // Complex type configurations
            builder.OwnsOne(x => x.Settings, settingsBuilder =>
            {
                settingsBuilder.Property(s => s.Title).HasColumnName("Title").HasMaxLength(AnalyticsReportingConsts.MaxNameLength);
                settingsBuilder.Property(s => s.Subtitle).HasColumnName("Subtitle").HasMaxLength(AnalyticsReportingConsts.MaxNameLength);
                settingsBuilder.Property(s => s.RefreshIntervalSeconds).HasColumnName("WidgetRefreshIntervalSeconds");
                settingsBuilder.Property(s => s.Configuration).HasColumnName("WidgetConfiguration");
                settingsBuilder.Property(s => s.ShowTitle).HasColumnName("ShowTitle");
                settingsBuilder.Property(s => s.AllowMaximize).HasColumnName("AllowMaximize");
            });

            builder.OwnsOne(x => x.VisualizationConfig, vizBuilder =>
            {
                vizBuilder.Property(v => v.ChartType).HasColumnName("ChartType").HasConversion<string>();
                vizBuilder.Property(v => v.ChartConfig).HasColumnName("ChartConfig");
                vizBuilder.Property(v => v.XAxisField).HasColumnName("XAxisField").HasMaxLength(100);
                vizBuilder.Property(v => v.ShowLegend).HasColumnName("ShowLegend");
                
                vizBuilder.Property(v => v.YAxisFields)
                    .HasColumnName("YAxisFields")
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                    );
                
                vizBuilder.Property(v => v.CustomColors)
                    .HasColumnName("CustomColors")
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                    );
            });

            // Indexes
            builder.HasIndex(x => x.DashboardId);
            builder.HasIndex(x => x.DataSourceId);
        }
    }
}
