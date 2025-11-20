using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Exceptions;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Services
{
    /// <summary>
    /// Manages operations on dashboards
    /// </summary>
    public class DashboardManager : DomainService, IDashboardManager
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IDataSourceRepository _dataSourceRepository;
        private readonly IDashboardTemplateRepository _templateRepository;
        private readonly ILogger<DashboardManager> _logger;
        
        public DashboardManager(
            IDashboardRepository dashboardRepository,
            IDataSourceRepository dataSourceRepository,
            IDashboardTemplateRepository templateRepository,
            ILogger<DashboardManager> logger)
        {
            _dashboardRepository = dashboardRepository;
            _dataSourceRepository = dataSourceRepository;
            _templateRepository = templateRepository;
            _logger = logger;
        }
        
        /// <summary>
        /// Creates a new dashboard
        /// </summary>
        public async Task<Dashboard> CreateDashboardAsync(
            Guid id,
            string name,
            string description,
            bool isPrivate,
            RefreshInterval refreshInterval,
            Guid? templateId = null,
            Guid? tenantId = null)
        {
            _logger.LogInformation("Creating dashboard {Name}", name);
            
            // Check for duplicate name
            if (await _dashboardRepository.ExistsByNameAsync(name, tenantId))
            {
                throw new DuplicateNameException(L["AnalyticsReporting:Entity:Dashboard"], name);
            }
            
            // If template is specified, validate it exists
            if (templateId.HasValue)
            {
                var template = await _templateRepository.GetAsync(templateId.Value);
                
                // Check if template is accessible for this tenant
                if (template.TenantId.HasValue && template.TenantId != tenantId && !template.IsSystem)
                {
                    throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidDashboardConfiguration")
                        .WithData("reason", "Template is not accessible for this tenant");
                }
            }
            
            var dashboard = Dashboard.Create(
                id,
                name,
                description,
                isPrivate,
                refreshInterval,
                tenantId
            );
            
            await _dashboardRepository.InsertAsync(dashboard);
            
            return dashboard;
        }
        
        /// <summary>
        /// Creates a dashboard from a template
        /// </summary>
        public async Task<Dashboard> CreateFromTemplateAsync(
            Guid id,
            string name,
            string description,
            bool isPrivate,
            Guid templateId,
            Guid? tenantId = null)
        {
            _logger.LogInformation("Creating dashboard {Name} from template {TemplateId}", name, templateId);
            
            // Check for duplicate name
            if (await _dashboardRepository.ExistsByNameAsync(name, tenantId))
            {
                throw new DuplicateNameException(L["AnalyticsReporting:Entity:Dashboard"], name);
            }
            
            // Get the template
            var template = await _templateRepository.GetAsync(templateId);
            
            // Check if template is accessible for this tenant
            if (template.TenantId.HasValue && template.TenantId != tenantId && !template.IsSystem)
            {
                throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidDashboardConfiguration")
                    .WithData("reason", "Template is not accessible for this tenant");
            }
            
            // Create refresh interval with default values
            var refreshInterval = new RefreshInterval(5, "minutes");
            
            // Create the dashboard
            var dashboard = Dashboard.Create(
                id,
                name,
                description,
                isPrivate,
                refreshInterval,
                tenantId,
                null,
                templateId
            );
            
            // Apply template configuration
            ApplyTemplateConfiguration(dashboard, template);
            
            await _dashboardRepository.InsertAsync(dashboard);
            
            return dashboard;
        }
        
        /// <summary>
        /// Applies template configuration to a dashboard
        /// </summary>
        private void ApplyTemplateConfiguration(Dashboard dashboard, DashboardTemplate template)
        {
            // This would parse the template's JSON configuration and apply it to the dashboard
            // In a real implementation, this would create widgets, set layout, etc. based on the template
            
            // Example implementation (placeholder):
            dashboard.ConfigureLayout(3, "grid", true);
            
            // Add example widgets from template
            // In reality, this would parse the template.TemplateJson
        }
        
        /// <summary>
        /// Adds a widget to a dashboard with data source validation
        /// </summary>
        public async Task<WidgetInstance> AddWidgetToDashboardAsync(
            Guid dashboardId,
            Guid widgetId,
            WidgetType type,
            WidgetSettings settings,
            int positionX,
            int positionY,
            int width,
            int height,
            Guid? dataSourceId = null,
            VisualizationConfiguration visualizationConfig = null)
        {
            var dashboard = await _dashboardRepository.GetAsync(dashboardId);
            
            // Validate data source if provided
            if (dataSourceId.HasValue)
            {
                var dataSource = await _dataSourceRepository.GetAsync(dataSourceId.Value);
                
                // Check if data source is accessible for this tenant
                if (dataSource.TenantId.HasValue && dataSource.TenantId != dashboard.TenantId)
                {
                    throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidWidgetConfiguration")
                        .WithData("reason", "Data source is not accessible for this dashboard");
                }
                
                // Check if data source is active
                if (!dataSource.IsActive)
                {
                    throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidWidgetConfiguration")
                        .WithData("reason", "Data source is not active");
                }
                
                // Validate data source type against widget type
                ValidateDataSourceForWidgetType(dataSource.Type, type);
            }
            
            // For chart widgets, ensure visualization config is provided
            if ((type == WidgetType.Chart || type == WidgetType.Heatmap || type == WidgetType.Timeline) 
                && visualizationConfig == null)
            {
                throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidWidgetConfiguration")
                    .WithData("reason", "Visualization configuration is required for chart widgets");
            }
            
            // Add widget to dashboard
            var widget = dashboard.AddWidget(
                widgetId,
                type,
                settings,
                positionX,
                positionY,
                width,
                height,
                dataSourceId,
                visualizationConfig
            );
            
            await _dashboardRepository.UpdateAsync(dashboard);
            
            return widget;
        }
        
        /// <summary>
        /// Validates that a data source type is compatible with a widget type
        /// </summary>
        private void ValidateDataSourceForWidgetType(DataSourceType dataSourceType, WidgetType widgetType)
        {
            // Implement validation logic for different combinations
            // This is a simplified example
            switch (widgetType)
            {
                case WidgetType.Chart:
                case WidgetType.Table:
                    // These widget types work with most data sources
                    break;
                    
                case WidgetType.KpiIndicator:
                    // KPI indicators might need specific data source types
                    if (dataSourceType != DataSourceType.AggregatedDataView && 
                        dataSourceType != DataSourceType.DatabaseView &&
                        dataSourceType != DataSourceType.API)
                    {
                        throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidWidgetConfiguration")
                            .WithData("reason", "KPI indicator widgets require aggregated data views, database views, or API data sources");
                    }
                    break;
                    
                // Add more cases for other widget types
            }
        }
        
        /// <summary>
        /// Creates a dashboard template from an existing dashboard
        /// </summary>
        public async Task<DashboardTemplate> CreateTemplateFromDashboardAsync(
            Guid templateId,
            Guid dashboardId,
            string name,
            string description,
            bool isSystem = false,
            string[] categories = null)
        {
            var dashboard = await _dashboardRepository.GetAsync(dashboardId);
            
            // Generate template JSON from dashboard
            string templateJson = GenerateTemplateJson(dashboard);
            
            // Create template
            var template = new DashboardTemplate(
                templateId,
                name,
                description,
                templateJson,
                dashboard.TenantId,
                isSystem,
                null,
                categories
            );
            
            await _templateRepository.InsertAsync(template);
            
            return template;
        }
        
        /// <summary>
        /// Generates template JSON from a dashboard
        /// </summary>
        private string GenerateTemplateJson(Dashboard dashboard)
        {
            // This would generate a JSON representation of the dashboard
            // In a real implementation, this would serialize the dashboard layout, widgets, etc.
            
            // Example implementation (placeholder)
            return "{ \"layout\": { \"columnCount\": 3 }, \"widgets\": [] }";
        }
        
        // Other implementation methods...
        
        private static string L(string name)
        {
            return LocalizationResource.GetString(AnalyticsReportingResource.ResourceName, name);
        }
    }
}
