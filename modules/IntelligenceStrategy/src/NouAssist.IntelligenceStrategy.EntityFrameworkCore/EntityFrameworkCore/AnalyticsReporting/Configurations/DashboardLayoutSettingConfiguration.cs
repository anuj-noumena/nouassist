using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for DashboardLayoutSetting entity
    /// </summary>
    public class DashboardLayoutSettingConfiguration : IEntityTypeConfiguration<DashboardLayoutSetting>
    {
        public void Configure(EntityTypeBuilder<DashboardLayoutSetting> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                AnalyticsReportingDbProperties.DbTablePrefix + "DashboardLayoutSettings",
                AnalyticsReportingDbProperties.DbSchema
            );

            // Key is defined in the relationship with Dashboard

            builder.Property(x => x.ColumnCount)
                .IsRequired();

            builder.Property(x => x.LayoutType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.CustomCss)
                .HasMaxLength(2000);

            builder.Property(x => x.LayoutConfig);

            // No need to configure the relationship to Dashboard as it's configured in DashboardConfiguration
        }
    }
}
