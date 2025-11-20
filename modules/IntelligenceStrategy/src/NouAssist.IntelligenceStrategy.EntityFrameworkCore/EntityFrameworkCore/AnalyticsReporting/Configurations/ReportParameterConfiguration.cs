using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for ReportParameter entity
    /// </summary>
    public class ReportParameterConfiguration : IEntityTypeConfiguration<ReportParameter>
    {
        public void Configure(EntityTypeBuilder<ReportParameter> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                AnalyticsReportingDbProperties.DbTablePrefix + "ReportParameters",
                AnalyticsReportingDbProperties.DbSchema
            );

            // The key is a composite key of ReportConfigurationId and Name
            builder.HasKey(x => new { x.ReportConfigurationId, x.Name });

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(AnalyticsReportingConsts.MaxNameLength);

            builder.Property(x => x.DataType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.DefaultValue)
                .HasMaxLength(AnalyticsReportingConsts.MaxNameLength);

            builder.Property(x => x.DisplayName)
                .HasMaxLength(AnalyticsReportingConsts.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(AnalyticsReportingConsts.MaxDescriptionLength);

            builder.Property(x => x.ValidationRules)
                .HasMaxLength(1000);

            // Indexes
            builder.HasIndex(x => x.ReportConfigurationId);
        }
    }
}
