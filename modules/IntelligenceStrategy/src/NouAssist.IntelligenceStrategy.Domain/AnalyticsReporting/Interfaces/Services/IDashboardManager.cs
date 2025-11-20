using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Interfaces.Services
{
    /// <summary>
    /// Interface for dashboard management operations
    /// </summary>
    public interface IDashboardManager : IDomainService
    {
        /// <summary>
        /// Creates a new dashboard
        /// </summary>
        Task<Dashboard> CreateDashboardAsync(
            Guid id,
            string name,
            string description,
            bool isPrivate,
            RefreshInterval refreshInterval,
            Guid? templateId = null,
            Guid? tenantId = null);
            
        /// <summary>
        /// Creates a dashboard from a template
        /// </summary>
        Task<Dashboard> CreateFromTemplateAsync(
            Guid id,
            string name,
            string description,
            bool isPrivate,
            Guid templateId,
            Guid? tenantId = null);
            
        /// <summary>
        /// Adds a widget to a dashboard with data source validation
        /// </summary>
        Task<WidgetInstance> AddWidgetToDashboardAsync(
            Guid dashboardId,
            Guid widgetId,
            WidgetType type,
            WidgetSettings settings,
            int positionX,
            int positionY,
            int width,
            int height,
            Guid? dataSourceId = null,
            VisualizationConfiguration visualizationConfig = null);
            
        /// <summary>
        /// Creates a dashboard template from an existing dashboard
        /// </summary>
        Task<DashboardTemplate> CreateTemplateFromDashboardAsync(
            Guid templateId,
            Guid dashboardId,
            string name,
            string description,
            bool isSystem = false,
            string[] categories = null);
    }
}
