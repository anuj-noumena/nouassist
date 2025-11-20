using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for DashboardTemplate entity
    /// </summary>
    public class DashboardTemplateConfiguration : IEntityTypeConfiguration<DashboardTemplate>
    {
        public void Configure(EntityTypeBuilder<DashboardTemplate> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                AnalyticsReportingDbProperties.DbTablePrefix + "DashboardTemplates",
                AnalyticsReportingDbProperties.DbSchema
            );

            // Primary key is handled by Entity<TKey>

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(AnalyticsReportingConsts.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(AnalyticsReportingConsts.MaxDescriptionLength);

            builder.Property(x => x.ThumbnailUrl)
                .HasMaxLength(1000);

            builder.Property(x => x.TemplateJson)
                .IsRequired();

            // Array property
            builder.Property(x => x.Categories)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                );

            // Indexes
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.IsSystem);
        }
    }
}
