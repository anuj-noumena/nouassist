using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.EntityFrameworkCore
{
    /// <summary>
    /// DbContext implementation for the AnalyticsReporting module
    /// </summary>
    [ConnectionStringName(AnalyticsReportingDbProperties.ConnectionStringName)]
    public class AnalyticsReportingDbContext : AbpDbContext<AnalyticsReportingDbContext>, IAnalyticsReportingDbContext
    {
        #region Aggregate Root DbSets
        public DbSet<Dashboard> Dashboards { get; set; } = null!;
        public DbSet<ReportConfiguration> ReportConfigurations { get; set; } = null!;
        public DbSet<KpiDefinition> KpiDefinitions { get; set; } = null!;
        public DbSet<DataSource> DataSources { get; set; } = null!;
        #endregion

        #region Entity DbSets
        public DbSet<DashboardTemplate> DashboardTemplates { get; set; } = null!;
        public DbSet<WidgetInstance> WidgetInstances { get; set; } = null!;
        public DbSet<ReportGenerationLog> ReportGenerationLogs { get; set; } = null!;
        public DbSet<KpiAlertLog> KpiAlertLogs { get; set; } = null!;
        public DbSet<AggregatedDataView> AggregatedDataViews { get; set; } = null!;
        #endregion
        
        public AnalyticsReportingDbContext(DbContextOptions<AnalyticsReportingDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // IMPORTANT: Call base first to apply ABP conventions
            base.OnModelCreating(builder);

            // Apply all configurations defined in this assembly
            builder.ConfigureAnalyticsReporting();
        }
    }
}
