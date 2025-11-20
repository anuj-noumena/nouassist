using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.EntityFrameworkCore
{
    /// <summary>
    /// DbContext interface for the AnalyticsReporting module
    /// </summary>
    [ConnectionStringName(AnalyticsReportingDbProperties.ConnectionStringName)]
    public interface IAnalyticsReportingDbContext : IEfCoreDbContext
    {
        // Aggregate Root DbSets
        DbSet<Dashboard> Dashboards { get; }
        DbSet<ReportConfiguration> ReportConfigurations { get; }
        DbSet<KpiDefinition> KpiDefinitions { get; }
        DbSet<DataSource> DataSources { get; }

        // Entity DbSets
        DbSet<DashboardTemplate> DashboardTemplates { get; }
        DbSet<WidgetInstance> WidgetInstances { get; }
        DbSet<ReportGenerationLog> ReportGenerationLogs { get; }
        DbSet<KpiAlertLog> KpiAlertLogs { get; }
        DbSet<AggregatedDataView> AggregatedDataViews { get; }
    }
}
