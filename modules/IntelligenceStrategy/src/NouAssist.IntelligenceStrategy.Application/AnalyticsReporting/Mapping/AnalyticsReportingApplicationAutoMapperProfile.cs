using AutoMapper;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Dashboards.Dtos;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.DataSources.Dtos;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Kpis.Dtos;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Reports.Dtos;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Mapping
{
    /// <summary>
    /// AutoMapper profile for the AnalyticsReporting module
    /// </summary>
    public class AnalyticsReportingApplicationAutoMapperProfile : Profile
    {
        public AnalyticsReportingApplicationAutoMapperProfile()
        {
            // Dashboard mappings
            CreateMap<Dashboard, DashboardDto>();
            CreateMap<DashboardLayoutSetting, DashboardLayoutSettingDto>();
            CreateMap<WidgetInstance, WidgetInstanceDto>();
            CreateMap<WidgetSettings, WidgetSettingsDto>();
            CreateMap<VisualizationConfiguration, VisualizationConfigurationDto>();
            CreateMap<RefreshInterval, RefreshIntervalDto>();
            
            // Report mappings
            CreateMap<ReportConfiguration, ReportConfigurationDto>();
            CreateMap<ReportParameter, ReportParameterDto>();
            CreateMap<ReportSchedule, ReportScheduleDto>();
            CreateMap<ScheduleTiming, ScheduleTimingDto>();
            CreateMap<ExportSettings, ExportSettingsDto>();
            CreateMap<ReportGenerationLog, ReportGenerationLogDto>();
            
            // KPI mappings
            CreateMap<KpiDefinition, KpiDefinitionDto>();
            CreateMap<KpiThreshold, KpiThresholdDto>();
            CreateMap<KpiValue, KpiValueDto>();
            CreateMap<AlertConditionLogic, AlertConditionLogicDto>();
            CreateMap<KpiAlertLog, KpiAlertLogDto>();
            
            // DataSource mappings
            CreateMap<DataSource, DataSourceDto>();
            CreateMap<DataTransformationStep, DataTransformationStepDto>();
            CreateMap<TransformationRule, TransformationRuleDto>();
            CreateMap<AggregatedDataView, AggregatedDataViewDto>();
            
            // Create Value Object mappings
            CreateMap<CreateDataSourceDto, DataConnectionString>()
                .ConstructUsing(src => 
                    src.ConnectionString != null 
                        ? new DataConnectionString(
                            src.ConnectionString.Value,
                            src.ConnectionString.ConnectionType,
                            src.ConnectionString.IsEncrypted)
                        : null)
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<CreateDataSourceDto, DataSourceCredentials>()
                .ConstructUsing(src => 
                    src.Credentials != null 
                        ? new DataSourceCredentials(
                            "ENCRYPTED:" + src.Credentials.Credentials,
                            DateTime.UtcNow,
                            true)
                        : null)
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<CreateDataSourceDto, RefreshInterval>()
                .ConstructUsing(src => 
                    new RefreshInterval(
                        src.RefreshIntervalValue,
                        src.RefreshIntervalUnit,
                        src.AutoRefreshEnabled))
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<UpdateDataSourceDto, RefreshInterval>()
                .ConstructUsing(src => 
                    new RefreshInterval(
                        src.RefreshIntervalValue,
                        src.RefreshIntervalUnit,
                        src.AutoRefreshEnabled))
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<UpdateDataSourceDto, DataConnectionString>()
                .ConstructUsing(src => 
                    src.ConnectionString != null 
                        ? new DataConnectionString(
                            src.ConnectionString.Value,
                            src.ConnectionString.ConnectionType,
                            src.ConnectionString.IsEncrypted)
                        : null)
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<UpdateCredentialsDto, DataSourceCredentials>()
                .ConstructUsing(src => 
                    new DataSourceCredentials(
                        "ENCRYPTED:" + src.Credentials,
                        DateTime.UtcNow,
                        true))
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<AddTransformationStepDto, TransformationRule>()
                .ConstructUsing(src => 
                    new TransformationRule(
                        src.Name,
                        src.SourceExpression,
                        src.TargetField,
                        src.TransformationExpression,
                        src.ExecutionOrder))
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<UpdateTransformationStepDto, TransformationRule>()
                .ConstructUsing((src, context) => 
                {
                    var existingRule = context.Items["ExistingRule"] as TransformationRule;
                    return new TransformationRule(
                        src.Name ?? existingRule.Name,
                        src.SourceExpression ?? existingRule.SourceExpression,
                        src.TargetField ?? existingRule.TargetField,
                        src.TransformationExpression ?? existingRule.TransformationExpression,
                        src.ExecutionOrder ?? existingRule.ExecutionOrder);
                })
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<CreateAggregatedDataViewDto, RefreshInterval>()
                .ConstructUsing(src => 
                    new RefreshInterval(
                        src.RefreshIntervalValue,
                        src.RefreshIntervalUnit,
                        src.AutoRefreshEnabled))
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<UpdateAggregatedDataViewDto, RefreshInterval>()
                .ConstructUsing(src => 
                    new RefreshInterval(
                        src.RefreshIntervalValue,
                        src.RefreshIntervalUnit,
                        src.AutoRefreshEnabled))
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<AddWidgetToDashboardDto, WidgetSettings>()
                .ConstructUsing(src => 
                    new WidgetSettings(
                        src.Title,
                        src.Subtitle,
                        src.RefreshIntervalSeconds,
                        src.Configuration,
                        src.ShowTitle,
                        src.AllowMaximize))
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<AddWidgetToDashboardDto, VisualizationConfiguration>()
                .ConstructUsing(src => 
                    src.VisualizationConfig != null 
                        ? new VisualizationConfiguration(
                            src.VisualizationConfig.ChartType,
                            src.VisualizationConfig.ChartConfig,
                            src.VisualizationConfig.XAxisField,
                            src.VisualizationConfig.YAxisFields,
                            src.VisualizationConfig.ShowLegend,
                            src.VisualizationConfig.CustomColors)
                        : null)
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<UpdateWidgetDto, WidgetSettings>()
                .ConstructUsing((src, context) => 
                {
                    var existingSettings = context.Items["ExistingSettings"] as WidgetSettings;
                    return new WidgetSettings(
                        src.Title ?? existingSettings.Title,
                        src.Subtitle ?? existingSettings.Subtitle,
                        src.RefreshIntervalSeconds ?? existingSettings.RefreshIntervalSeconds,
                        src.Configuration ?? existingSettings.Configuration,
                        src.ShowTitle ?? existingSettings.ShowTitle,
                        src.AllowMaximize ?? existingSettings.AllowMaximize);
                })
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<UpdateWidgetDto, VisualizationConfiguration>()
                .ConstructUsing((src, context) => 
                {
                    if (src.VisualizationConfig == null)
                        return context.Items["ExistingVisConfig"] as VisualizationConfiguration;
                        
                    var existingConfig = context.Items["ExistingVisConfig"] as VisualizationConfiguration;
                    return new VisualizationConfiguration(
                        src.VisualizationConfig.ChartType,
                        src.VisualizationConfig.ChartConfig ?? existingConfig?.ChartConfig,
                        src.VisualizationConfig.XAxisField ?? existingConfig?.XAxisField,
                        src.VisualizationConfig.YAxisFields ?? existingConfig?.YAxisFields,
                        src.VisualizationConfig.ShowLegend,
                        src.VisualizationConfig.CustomColors ?? existingConfig?.CustomColors);
                })
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<CreateDashboardDto, RefreshInterval>()
                .ConstructUsing(src => 
                    new RefreshInterval(
                        src.RefreshIntervalValue,
                        src.RefreshIntervalUnit,
                        src.AutoRefreshEnabled))
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<UpdateDashboardDto, RefreshInterval>()
                .ConstructUsing(src => 
                    new RefreshInterval(
                        src.RefreshIntervalValue,
                        src.RefreshIntervalUnit,
                        src.AutoRefreshEnabled))
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<CreateKpiDefinitionDto, RefreshInterval>()
                .ConstructUsing(src => 
                    new RefreshInterval(
                        src.RefreshIntervalValue,
                        src.RefreshIntervalUnit,
                        src.AutoRefreshEnabled))
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<UpdateKpiDefinitionDto, RefreshInterval>()
                .ConstructUsing(src => 
                    new RefreshInterval(
                        src.RefreshIntervalValue,
                        src.RefreshIntervalUnit,
                        src.AutoRefreshEnabled))
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<AddKpiThresholdDto, AlertConditionLogic>()
                .ConstructUsing(src => 
                    !string.IsNullOrEmpty(src.AlertExpression) 
                        ? new AlertConditionLogic(
                            src.AlertExpression,
                            src.AlertDescription ?? "Alert condition",
                            src.RequireConsecutiveOccurrences,
                            src.ConsecutiveOccurrencesRequired)
                        : null)
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<UpdateKpiThresholdDto, AlertConditionLogic>()
                .ConstructUsing((src, context) => 
                {
                    if (string.IsNullOrEmpty(src.AlertExpression))
                        return context.Items["ExistingAlertCondition"] as AlertConditionLogic;
                        
                    var existingCondition = context.Items["ExistingAlertCondition"] as AlertConditionLogic;
                    return new AlertConditionLogic(
                        src.AlertExpression,
                        src.AlertDescription ?? existingCondition?.Description ?? "Alert condition",
                        src.RequireConsecutiveOccurrences ?? existingCondition?.RequireConsecutiveOccurrences ?? false,
                        src.ConsecutiveOccurrencesRequired ?? existingCondition?.ConsecutiveOccurrencesRequired ?? 1);
                })
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<CreateExportSettingsDto, ExportSettings>()
                .ConstructUsing(src => 
                    new ExportSettings(
                        src.Format,
                        src.IncludeCharts,
                        src.IncludeRawData,
                        src.IncludeTimestampInFilename,
                        src.FilenameTemplate))
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<UpdateExportSettingsDto, ExportSettings>()
                .ConstructUsing(src => 
                    new ExportSettings(
                        src.Format,
                        src.IncludeCharts,
                        src.IncludeRawData,
                        src.IncludeTimestampInFilename,
                        src.FilenameTemplate))
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<AddReportScheduleDto, ScheduleTiming>()
                .ConstructUsing(src => 
                    new ScheduleTiming(
                        src.CronExpression,
                        src.TimeZone,
                        src.StartDate,
                        src.EndDate,
                        src.IsActive))
                .ForAllMembers(opts => opts.Ignore());
                
            CreateMap<UpdateReportScheduleDto, ScheduleTiming>()
                .ConstructUsing((src, context) => 
                {
                    var existingTiming = context.Items["ExistingTiming"] as ScheduleTiming;
                    return new ScheduleTiming(
                        src.CronExpression ?? existingTiming.CronExpression,
                        src.TimeZone ?? existingTiming.TimeZone,
                        src.StartDate ?? existingTiming.StartDate,
                        src.EndDate ?? existingTiming.EndDate,
                        src.IsActive ?? existingTiming.IsActive);
                })
                .ForAllMembers(opts => opts.Ignore());
        }
    }
}
