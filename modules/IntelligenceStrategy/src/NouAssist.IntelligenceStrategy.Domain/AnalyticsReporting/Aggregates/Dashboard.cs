using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Events;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Exceptions;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates
{
    /// <summary>
    /// A user-defined dashboard configuration with widgets
    /// </summary>
    public class Dashboard : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Optional tenant ID for multi-tenancy
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Name of the dashboard
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Description of the dashboard
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Icon for the dashboard
        /// </summary>
        public string Icon { get; private set; }
        
        /// <summary>
        /// Whether this dashboard is private to the creator
        /// </summary>
        public bool IsPrivate { get; private set; }
        
        /// <summary>
        /// Layout settings for the dashboard
        /// </summary>
        public DashboardLayoutSetting LayoutSetting { get; private set; }
        
        /// <summary>
        /// The dashboard template this was created from, if any
        /// </summary>
        public Guid? TemplateId { get; private set; }
        
        /// <summary>
        /// Tags for organizing dashboards
        /// </summary>
        public string[] Tags { get; private set; }
        
        /// <summary>
        /// Refresh interval for the dashboard
        /// </summary>
        public RefreshInterval RefreshInterval { get; private set; }
        
        private readonly List<WidgetInstance> _widgets;
        
        /// <summary>
        /// Widgets placed on the dashboard
        /// </summary>
        public IReadOnlyCollection<WidgetInstance> Widgets => _widgets.AsReadOnly();
        
        protected Dashboard()
        {
            _widgets = new List<WidgetInstance>();
            Tags = Array.Empty<string>();
        }
        
        /// <summary>
        /// Creates a new dashboard
        /// </summary>
        public static Dashboard Create(
            Guid id,
            string name,
            string description,
            bool isPrivate,
            RefreshInterval refreshInterval,
            Guid? tenantId = null,
            string icon = null,
            Guid? templateId = null,
            string[] tags = null)
        {
            var dashboard = new Dashboard(
                id,
                name,
                description,
                isPrivate,
                refreshInterval,
                tenantId,
                icon,
                templateId,
                tags
            );
            
            dashboard.AddDistributedEvent(
                new DashboardCreatedEto
                {
                    Id = dashboard.Id,
                    Name = dashboard.Name,
                    CreatorId = dashboard.CreatorId.GetValueOrDefault(),
                    IsPrivate = dashboard.IsPrivate,
                    TenantId = dashboard.TenantId
                }
            );
            
            return dashboard;
        }
        
        private Dashboard(
            Guid id,
            string name,
            string description,
            bool isPrivate,
            RefreshInterval refreshInterval,
            Guid? tenantId = null,
            string icon = null,
            Guid? templateId = null,
            string[] tags = null)
            : base(id)
        {
            SetName(name);
            SetDescription(description);
            IsPrivate = isPrivate;
            RefreshInterval = refreshInterval;
            TenantId = tenantId;
            Icon = icon;
            TemplateId = templateId;
            Tags = tags ?? Array.Empty<string>();
            _widgets = new List<WidgetInstance>();
        }
        
        /// <summary>
        /// Sets the dashboard name
        /// </summary>
        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), AnalyticsReportingConsts.MaxNameLength);
        }
        
        /// <summary>
        /// Sets the dashboard description
        /// </summary>
        public void SetDescription(string description)
        {
            Description = description;
        }
        
        /// <summary>
        /// Sets the dashboard icon
        /// </summary>
        public void SetIcon(string icon)
        {
            Icon = icon;
        }
        
        /// <summary>
        /// Sets the privacy of the dashboard
        /// </summary>
        public void SetPrivacy(bool isPrivate)
        {
            IsPrivate = isPrivate;
        }
        
        /// <summary>
        /// Sets tags for the dashboard
        /// </summary>
        public void SetTags(string[] tags)
        {
            Tags = tags ?? Array.Empty<string>();
        }
        
        /// <summary>
        /// Updates the refresh interval
        /// </summary>
        public void UpdateRefreshInterval(RefreshInterval refreshInterval)
        {
            RefreshInterval = refreshInterval;
        }
        
        /// <summary>
        /// Configures the dashboard layout
        /// </summary>
        public void ConfigureLayout(
            int columnCount,
            string layoutType,
            bool autoResize = true,
            string customCss = null,
            string layoutConfig = null)
        {
            if (LayoutSetting == null)
            {
                LayoutSetting = new DashboardLayoutSetting(
                    Id,
                    columnCount,
                    layoutType,
                    autoResize,
                    customCss,
                    layoutConfig
                );
            }
            else
            {
                LayoutSetting.SetColumnCount(columnCount);
                LayoutSetting.SetLayoutType(layoutType);
                LayoutSetting.SetAutoResize(autoResize);
                LayoutSetting.SetCustomCss(customCss);
                LayoutSetting.SetLayoutConfig(layoutConfig);
            }
            
            AddDistributedEvent(
                new DashboardLayoutUpdatedEto
                {
                    Id = Id,
                    TenantId = TenantId
                }
            );
        }
        
        /// <summary>
        /// Adds a widget to the dashboard
        /// </summary>
        public WidgetInstance AddWidget(
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
            var widget = new WidgetInstance(
                widgetId,
                Id,
                type,
                settings,
                positionX,
                positionY,
                width,
                height,
                dataSourceId,
                visualizationConfig
            );
            
            _widgets.Add(widget);
            
            AddDistributedEvent(
                new WidgetAddedEto
                {
                    Id = widget.Id,
                    DashboardId = Id,
                    WidgetType = (int)type,
                    TenantId = TenantId
                }
            );
            
            return widget;
        }
        
        /// <summary>
        /// Removes a widget from the dashboard
        /// </summary>
        public void RemoveWidget(Guid widgetId)
        {
            var widget = GetWidget(widgetId);
            _widgets.Remove(widget);
            
            AddDistributedEvent(
                new WidgetRemovedEto
                {
                    Id = widgetId,
                    DashboardId = Id,
                    TenantId = TenantId
                }
            );
        }
        
        /// <summary>
        /// Updates a widget's position
        /// </summary>
        public void UpdateWidgetPosition(Guid widgetId, int positionX, int positionY)
        {
            var widget = GetWidget(widgetId);
            widget.SetPosition(positionX, positionY);
            
            AddDistributedEvent(
                new WidgetUpdatedEto
                {
                    Id = widgetId,
                    DashboardId = Id,
                    TenantId = TenantId
                }
            );
        }
        
        /// <summary>
        /// Updates a widget's size
        /// </summary>
        public void UpdateWidgetSize(Guid widgetId, int width, int height)
        {
            var widget = GetWidget(widgetId);
            widget.SetSize(width, height);
            
            AddDistributedEvent(
                new WidgetUpdatedEto
                {
                    Id = widgetId,
                    DashboardId = Id,
                    TenantId = TenantId
                }
            );
        }
        
        /// <summary>
        /// Updates a widget's settings
        /// </summary>
        public void UpdateWidgetSettings(Guid widgetId, WidgetSettings settings)
        {
            var widget = GetWidget(widgetId);
            widget.UpdateSettings(settings);
            
            AddDistributedEvent(
                new WidgetUpdatedEto
                {
                    Id = widgetId,
                    DashboardId = Id,
                    TenantId = TenantId
                }
            );
        }
        
        /// <summary>
        /// Updates a chart widget's visualization configuration
        /// </summary>
        public void UpdateWidgetVisualization(Guid widgetId, VisualizationConfiguration config)
        {
            var widget = GetWidget(widgetId);
            widget.SetVisualizationConfig(config);
            
            AddDistributedEvent(
                new WidgetUpdatedEto
                {
                    Id = widgetId,
                    DashboardId = Id,
                    TenantId = TenantId
                }
            );
        }
        
        /// <summary>
        /// Updates a widget's data source
        /// </summary>
        public void UpdateWidgetDataSource(Guid widgetId, Guid dataSourceId)
        {
            var widget = GetWidget(widgetId);
            widget.SetDataSource(dataSourceId);
            
            AddDistributedEvent(
                new WidgetUpdatedEto
                {
                    Id = widgetId,
                    DashboardId = Id,
                    TenantId = TenantId
                }
            );
        }
        
        /// <summary>
        /// Gets a widget by ID
        /// </summary>
        private WidgetInstance GetWidget(Guid widgetId)
        {
            var widget = _widgets.FirstOrDefault(w => w.Id == widgetId);
            if (widget == null)
            {
                throw new EntityNotFoundException("Widget", widgetId);
            }
            return widget;
        }
    }
}
