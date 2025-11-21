using System;
using System.Threading.Tasks;
using NouAssist.Analytics.Aggregates;
using NouAssist.Analytics.Entities;
using NouAssist.Analytics.Enums;
using NouAssist.Analytics.Interfaces.Services;
using NouAssist.Analytics.ValueObjects;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Interfaces.Services;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace NouAssist.Analytics.Data
{
    /// <summary>
    /// Seeds initial data for the Analytics module
    /// </summary>
    public class AnalyticsDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IDashboardManager _dashboardManager;
        private readonly IReportManager _reportManager;
        private readonly IKpiManager _kpiManager;
        private readonly IDataSourceConnector _dataSourceConnector;
        private readonly IGuidGenerator _guidGenerator;
        
        public AnalyticsDataSeedContributor(
            IDashboardManager dashboardManager,
            IReportManager reportManager,
            IKpiManager kpiManager,
            IDataSourceConnector dataSourceConnector,
            IGuidGenerator guidGenerator)
        {
            _dashboardManager = dashboardManager;
            _reportManager = reportManager;
            _kpiManager = kpiManager;
            _dataSourceConnector = dataSourceConnector;
            _guidGenerator = guidGenerator;
        }
        
        public async Task SeedAsync(DataSeedContext context)
        {
            // Seed default data sources
            var projectsDataSourceId = await SeedProjectDataSourceAsync();
            var workflowsDataSourceId = await SeedWorkflowDataSourceAsync();
            
            // Seed default KPIs
            await SeedKpisAsync(projectsDataSourceId, workflowsDataSourceId);
            
            // Seed default reports
            await SeedReportsAsync(projectsDataSourceId, workflowsDataSourceId);
            
            // Seed default dashboard
            await SeedDefaultDashboardAsync(projectsDataSourceId, workflowsDataSourceId);
        }
        
        private async Task<Guid> SeedProjectDataSourceAsync()
        {
            var dataSource = await _dataSourceConnector.CreateDataSourceAsync(
                "Project Events",
                "Aggregated project tasks and milestones data from Project BC",
                DataSourceType.EventStream,
                new DataConnectionString("EventBus:ProjectEvents"),
                new DataSourceCredentials(),
                null // No tenant - system-wide
            );
            
            // Add transformation steps
            await _dataSourceConnector.AddTransformationStepAsync(
                dataSource.Id,
                "Filter Completed Tasks",
                "FilterTransformation",
                @"{""eventType"": ""TaskStatusUpdated"", ""filter"": {""Status"": ""Completed""}}",
                0
            );
            
            await _dataSourceConnector.AddTransformationStepAsync(
                dataSource.Id,
                "Aggregate By Project",
                "AggregationTransformation",
                @"{""groupBy"": ""ProjectId"", ""metrics"": [""Count"", ""Sum"", ""Average""]}",
                1
            );
            
            // Add aggregated view
            await _dataSourceConnector.AddAggregatedDataViewAsync(
                dataSource.Id,
                "ProjectProgress",
                @"{""table"": ""ProjectProgressView"", ""columns"": [""ProjectId"", ""ProjectName"", ""CompletedTasks"", ""TotalTasks"", ""PercentComplete"", ""LastUpdated""]}",
                "RefreshOnEvent"
            );
            
            return dataSource.Id;
        }
        
        private async Task<Guid> SeedWorkflowDataSourceAsync()
        {
            var dataSource = await _dataSourceConnector.CreateDataSourceAsync(
                "Workflow Events",
                "Workflow execution and performance data from Workflow Engine BC",
                DataSourceType.EventStream,
                new DataConnectionString("EventBus:WorkflowEvents"),
                new DataSourceCredentials(),
                null // No tenant - system-wide
            );
            
            // Add transformation steps
            await _dataSourceConnector.AddTransformationStepAsync(
                dataSource.Id,
                "Calculate Cycle Times",
                "CalculationTransformation",
                @"{""formula"": ""TIMEDIFF(WorkflowInstanceStarted.Timestamp, WorkflowInstanceCompleted.Timestamp, 'SECONDS')"", ""outputField"": ""CycleTimeSeconds""}",
                0
            );
            
            await _dataSourceConnector.AddTransformationStepAsync(
                dataSource.Id,
                "Identify Bottlenecks",
                "AnalyticsTransformation",
                @"{""algorithm"": ""BottleneckDetection"", ""parameters"": {""threshold"": 120}}",
                1
            );
            
            // Add aggregated view
            await _dataSourceConnector.AddAggregatedDataViewAsync(
                dataSource.Id,
                "WorkflowPerformance",
                @"{""table"": ""WorkflowPerformanceView"", ""columns"": [""WorkflowId"", ""WorkflowName"", ""AvgCycleTime"", ""MaxCycleTime"", ""CompletionRate"", ""BottleneckActivities""]}",
                "RefreshHourly"
            );
            
            return dataSource.Id;
        }
        
        private async Task SeedKpisAsync(Guid projectsDataSourceId, Guid workflowsDataSourceId)
        {
            // Create Project Completion Rate KPI
            var projectCompletionKpi = await _kpiManager.CreateKpiDefinitionAsync(
                "Project Completion Rate",
                "Percentage of projects completed on time vs. total projects due",
                "SUM(CompletedOnTime) / SUM(TotalDue) * 100",
                null // No tenant - system-wide
            );
            
            await _kpiManager.AddDataSourceToKpiAsync(projectCompletionKpi.Id, projectsDataSourceId);
            
            // Add thresholds
            await _kpiManager.AddKpiThresholdAsync(
                projectCompletionKpi.Id,
                "Critical Low Completion",
                75.0m,
                false, // Lower bound
                KpiStatus.Critical,
                3, // Highest severity
                new AlertConditionLogic("value <= threshold", 1, 60, true),
                "Project completion rate has fallen below critical threshold of 75%"
            );
            
            await _kpiManager.AddKpiThresholdAsync(
                projectCompletionKpi.Id,
                "Warning Low Completion",
                85.0m,
                false, // Lower bound
                KpiStatus.Warning,
                2, // Medium severity
                new AlertConditionLogic("value <= threshold", 2, 120, true),
                "Project completion rate is below target at {0}%"
            );
            
            // Create Workflow Efficiency KPI
            var workflowEfficiencyKpi = await _kpiManager.CreateKpiDefinitionAsync(
                "Workflow Efficiency Index",
                "Composite score of workflow cycle times and completion rates",
                "(100 - AVG(NormalizedCycleTime) * 0.6) + (AVG(CompletionRate) * 0.4)",
                null // No tenant - system-wide
            );
            
            await _kpiManager.AddDataSourceToKpiAsync(workflowEfficiencyKpi.Id, workflowsDataSourceId);
            
            // Add thresholds
            await _kpiManager.AddKpiThresholdAsync(
                workflowEfficiencyKpi.Id,
                "Low Efficiency",
                60.0m,
                false, // Lower bound
                KpiStatus.Critical,
                3, // Highest severity
                new AlertConditionLogic("value <= threshold", 2, 60, true),
                "Workflow efficiency has dropped to a critical level"
            );
            
            await _kpiManager.AddKpiThresholdAsync(
                workflowEfficiencyKpi.Id,
                "Medium Efficiency",
                75.0m,
                false, // Lower bound
                KpiStatus.Warning,
                2, // Medium severity
                new AlertConditionLogic("value <= threshold", 3, 240, true),
                "Workflow efficiency needs improvement"
            );
        }
        
        private async Task SeedReportsAsync(Guid projectsDataSourceId, Guid workflowsDataSourceId)
        {
            // Create Project Progress Report
            var projectProgressReport = await _reportManager.CreateReportConfigurationAsync(
                "Project Progress Report",
                "Detailed report of project status, milestones, and task completion rates",
                ReportFormat.Pdf,
                new ReportSchedule(
                    true,
                    "0 0 * * 1", // Every Monday at midnight
                    DateTime.UtcNow.AddDays(7).Date, // Next Monday
                    "reports@example.com",
                    true
                ),
                null // No tenant - system-wide
            );
            
            await _reportManager.AddDataSourceToReportAsync(projectProgressReport.Id, projectsDataSourceId);
            
            // Add parameters
            await _reportManager.AddReportParameterAsync(
                projectProgressReport.Id,
                "StartDate",
                "Start Date",
                "date",
                true,
                DateTime.UtcNow.AddMonths(-1).ToString("yyyy-MM-dd"),
                0
            );
            
            await _reportManager.AddReportParameterAsync(
                projectProgressReport.Id,
                "EndDate",
                "End Date",
                "date",
                true,
                DateTime.UtcNow.ToString("yyyy-MM-dd"),
                1
            );
            
            await _reportManager.AddReportParameterAsync(
                projectProgressReport.Id,
                "ProjectStatus",
                "Project Status",
                "enum",
                false,
                "All",
                2
            );
            
            // Create Workflow Performance Report
            var workflowReport = await _reportManager.CreateReportConfigurationAsync(
                "Workflow Performance Analysis",
                "Analysis of workflow execution times, bottlenecks, and optimization opportunities",
                ReportFormat.Excel,
                new ReportSchedule(
                    true,
                    "0 0 1 * *", // First day of each month at midnight
                    new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).AddMonths(1), // First day of next month
                    "process-owners@example.com",
                    true
                ),
                null // No tenant - system-wide
            );
            
            await _reportManager.AddDataSourceToReportAsync(workflowReport.Id, workflowsDataSourceId);
            
            // Add parameters
            await _reportManager.AddReportParameterAsync(
                workflowReport.Id,
                "WorkflowType",
                "Workflow Type",
                "enum",
                false,
                "All",
                0
            );
            
            await _reportManager.AddReportParameterAsync(
                workflowReport.Id,
                "DateRange",
                "Date Range",
                "string",
                true,
                "LastMonth",
                1,
                "^(LastWeek|LastMonth|LastQuarter|LastYear|Custom)$"
            );
            
            await _reportManager.AddReportParameterAsync(
                workflowReport.Id,
                "IncludeBottleneckAnalysis",
                "Include Bottleneck Analysis",
                "boolean",
                false,
                "true",
                2
            );
        }
        
        private async Task SeedDefaultDashboardAsync(Guid projectsDataSourceId, Guid workflowsDataSourceId)
        {
            // Create default dashboard
            var dashboard = await _dashboardManager.CreateDashboardAsync(
                "Executive Overview",
                "High-level metrics and KPIs for organizational performance",
                true, // Set as default
                new DashboardLayoutSetting(
                    4, // 4 columns
                    "corporate",
                    true, // Auto refresh
                    300, // Every 5 minutes
                    "",
                    false
                ),
                null // No tenant - system-wide
            );
            
            // Add project progress widget
            await _dashboardManager.AddWidgetToDashboardAsync(
                dashboard.Id,
                "Project Completion Trend",
                WidgetType.Chart,
                new WidgetSettings(
                    "SELECT Date, CompletionRate FROM ProjectProgressView GROUP BY Date ORDER BY Date",
                    ChartType.Line,
                    true,
                    true,
                    "blue",
                    "yyyy-MM-dd",
                    "0.0%",
                    true,
                    300
                ),
                0, 0, 2, 1,
                projectsDataSourceId
            );
            
            // Add KPI indicator widget
            await _dashboardManager.AddWidgetToDashboardAsync(
                dashboard.Id,
                "Project Completion Rate",
                WidgetType.KpiIndicator,
                new WidgetSettings(
                    "SELECT CurrentValue, Status FROM KpiDefinitions WHERE Name = 'Project Completion Rate'",
                    null,
                    false,
                    true,
                    "default",
                    "yyyy-MM-dd",
                    "0.0%",
                    true,
                    300
                ),
                2, 0, 1, 1
            );
            
            // Add workflow performance widget
            await _dashboardManager.AddWidgetToDashboardAsync(
                dashboard.Id,
                "Workflow Performance by Type",
                WidgetType.Chart,
                new WidgetSettings(
                    "SELECT WorkflowType, AvgCycleTime FROM WorkflowPerformanceView GROUP BY WorkflowType",
                    ChartType.Bar,
                    true,
                    true,
                    "green",
                    "yyyy-MM-dd",
                    "0.0",
                    true,
                    300
                ),
                0, 1, 2, 1,
                workflowsDataSourceId
            );
            
            // Add bottleneck analysis widget
            await _dashboardManager.AddWidgetToDashboardAsync(
                dashboard.Id,
                "Process Bottlenecks",
                WidgetType.DataTable,
                new WidgetSettings(
                    "SELECT ActivityName, AvgDuration, FrequencyPercent FROM BottleneckActivities ORDER BY AvgDuration DESC LIMIT 5",
                    null,
                    false,
                    false,
                    "default",
                    "yyyy-MM-dd",
                    "0.0",
                    true,
                    300
                ),
                2, 1, 2, 1,
                workflowsDataSourceId
            );
        }
    }
}
