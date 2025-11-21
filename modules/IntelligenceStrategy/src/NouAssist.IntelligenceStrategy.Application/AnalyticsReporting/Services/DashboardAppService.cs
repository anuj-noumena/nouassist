using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Analytics.DTOs.Dashboard;
using NouAssist.Analytics.Entities;
using NouAssist.Analytics.Permissions;
using NouAssist.Analytics.Repositories;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Exceptions;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Interfaces.Services;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Analytics.Services
{
    [Authorize(AnalyticsPermissions.Dashboards.Default)]
    public class DashboardAppService : ApplicationService, IDashboardAppService
    {
        private readonly IDashboardManager _dashboardManager;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IRepository<DashboardTemplate, Guid> _dashboardTemplateRepository;
        private readonly IDataSourceRepository _dataSourceRepository;
        
        public DashboardAppService(
            IDashboardManager dashboardManager,
            IDashboardRepository dashboardRepository,
            IRepository<DashboardTemplate, Guid> dashboardTemplateRepository,
            IDataSourceRepository dataSourceRepository)
        {
            _dashboardManager = dashboardManager;
            _dashboardRepository = dashboardRepository;
            _dashboardTemplateRepository = dashboardTemplateRepository;
            _dataSourceRepository = dataSourceRepository;
        }
        
        public async Task<DashboardDto> GetAsync(Guid id)
        {
            var dashboard = await _dashboardRepository.GetAsync(id);
            return await MapDashboardToDtoAsync(dashboard);
        }
        
        public async Task<PagedResultDto<DashboardDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _dashboardRepository.CountAsync();
            
            var dashboards = await _dashboardRepository.GetPagedListAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting ?? "Name"
            );
            
            var dashboardDtos = new List<DashboardDto>();
            foreach (var dashboard in dashboards)
            {
                dashboardDtos.Add(await MapDashboardToDtoAsync(dashboard));
            }
            
            return new PagedResultDto<DashboardDto>(totalCount, dashboardDtos);
        }
        
        public async Task<DashboardDto> GetDefaultAsync()
        {
            var dashboard = await _dashboardRepository.GetDefaultDashboardAsync(CurrentTenant.Id);
            if (dashboard == null)
            {
                return null;
            }
            
            return await MapDashboardToDtoAsync(dashboard);
        }
        
        [Authorize(AnalyticsPermissions.Dashboards.Create)]
        public async Task<DashboardDto> CreateAsync(CreateDashboardDto input)
        {
            var layoutSetting = new DashboardLayoutSetting(
                input.LayoutSetting?.Columns ?? 4,
                input.LayoutSetting?.Theme ?? "default",
                input.LayoutSetting?.AutoRefresh ?? false,
                input.LayoutSetting?.RefreshIntervalSeconds ?? 300,
                input.LayoutSetting?.CustomCss ?? "",
                input.LayoutSetting?.CompactView ?? false
            );
            
            var dashboard = await _dashboardManager.CreateDashboardAsync(
                input.Name,
                input.Description,
                input.IsDefault,
                layoutSetting,
                CurrentTenant.Id
            );
            
            return await MapDashboardToDtoAsync(dashboard);
        }
        
        [Authorize(AnalyticsPermissions.Dashboards.Edit)]
        public async Task<DashboardDto> UpdateAsync(Guid id, UpdateDashboardDto input)
        {
            var dashboard = await _dashboardRepository.GetAsync(id);
            
            var layoutSetting = new DashboardLayoutSetting(
                input.LayoutSetting?.Columns ?? dashboard.LayoutSetting.Columns,
                input.LayoutSetting?.Theme ?? dashboard.LayoutSetting.Theme,
                input.LayoutSetting?.AutoRefresh ?? dashboard.LayoutSetting.AutoRefresh,
                input.LayoutSetting?.RefreshIntervalSeconds ?? dashboard.LayoutSetting.RefreshIntervalSeconds,
                input.LayoutSetting?.CustomCss ?? dashboard.LayoutSetting.CustomCss,
                input.LayoutSetting?.CompactView ?? dashboard.LayoutSetting.CompactView
            );
            
            var updatedDashboard = await _dashboardManager.UpdateDashboardAsync(
                id,
                input.Name,
                input.Description,
                input.IsDefault,
                layoutSetting
            );
            
            return await MapDashboardToDtoAsync(updatedDashboard);
        }
        
        [Authorize(AnalyticsPermissions.Dashboards.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _dashboardRepository.DeleteAsync(id);
        }
        
        [Authorize(AnalyticsPermissions.Dashboards.Edit)]
        public async Task<WidgetInstanceDto> AddWidgetAsync(Guid dashboardId, AddWidgetDto input)
        {
            var widgetSettings = new ValueObjects.WidgetSettings(
                input.Settings.DataQuery,
                input.Settings.ChartType,
                input.Settings.ShowLegend,
                input.Settings.ShowLabels,
                input.Settings.ColorScheme,
                input.Settings.DateFormat,
                input.Settings.NumberFormat,
                input.Settings.AutoRefresh,
                input.Settings.RefreshIntervalSeconds,
                input.Settings.AdditionalSettings
            );
            
            var widget = await _dashboardManager.AddWidgetToDashboardAsync(
                dashboardId,
                input.Title,
                input.Type,
                widgetSettings,
                input.PositionX,
                input.PositionY,
                input.Width,
                input.Height,
                input.DataSourceId
            );
            
            var dataSourceName = "";
            if (widget.DataSourceId.HasValue)
            {
                var dataSource = await _dataSourceRepository.FindAsync(widget.DataSourceId.Value);
                if (dataSource != null)
                {
                    dataSourceName = dataSource.Name;
                }
            }
            
            return new WidgetInstanceDto
            {
                Id = widget.Id,
                Title = widget.Title,
                Type = widget.Type,
                DataSourceId = widget.DataSourceId,
                DataSourceName = dataSourceName,
                Settings = MapWidgetSettingsToDto(widget.Settings),
                PositionX = widget.PositionX,
                PositionY = widget.PositionY,
                Width = widget.Width,
                Height = widget.Height
            };
        }
        
        [Authorize(AnalyticsPermissions.Dashboards.Edit)]
        public async Task<WidgetInstanceDto> UpdateWidgetAsync(Guid dashboardId, Guid widgetId, UpdateWidgetDto input)
        {
            var dashboard = await _dashboardRepository.GetAsync(dashboardId);
            var widget = dashboard.Widgets.FirstOrDefault(w => w.Id == widgetId);
            
            if (widget == null)
            {
                throw new AnalyticsDomainException("Dashboard:ErrorMessage:WidgetNotFound")
                    .WithData("widgetId", widgetId);
            }
            
            var widgetSettings = new ValueObjects.WidgetSettings(
                input.Settings.DataQuery,
                input.Settings.ChartType,
                input.Settings.ShowLegend,
                input.Settings.ShowLabels,
                input.Settings.ColorScheme,
                input.Settings.DateFormat,
                input.Settings.NumberFormat,
                input.Settings.AutoRefresh,
                input.Settings.RefreshIntervalSeconds,
                input.Settings.AdditionalSettings
            );
            
            widget.UpdateTitle(input.Title);
            widget.UpdateSettings(widgetSettings);
            widget.UpdatePosition(input.PositionX, input.PositionY);
            widget.UpdateSize(input.Width, input.Height);
            
            await _dashboardRepository.UpdateAsync(dashboard);
            
            var dataSourceName = "";
            if (widget.DataSourceId.HasValue)
            {
                var dataSource = await _dataSourceRepository.FindAsync(widget.DataSourceId.Value);
                if (dataSource != null)
                {
                    dataSourceName = dataSource.Name;
                }
            }
            
            return new WidgetInstanceDto
            {
                Id = widget.Id,
                Title = widget.Title,
                Type = widget.Type,
                DataSourceId = widget.DataSourceId,
                DataSourceName = dataSourceName,
                Settings = MapWidgetSettingsToDto(widget.Settings),
                PositionX = widget.PositionX,
                PositionY = widget.PositionY,
                Width = widget.Width,
                Height = widget.Height
            };
        }
        
        [Authorize(AnalyticsPermissions.Dashboards.Edit)]
        public async Task RemoveWidgetAsync(Guid dashboardId, Guid widgetId)
        {
            await _dashboardManager.RemoveWidgetFromDashboardAsync(dashboardId, widgetId);
        }
        
        [Authorize(AnalyticsPermissions.Dashboards.Create)]
        public async Task<DashboardTemplateDto> SaveAsTemplateAsync(Guid dashboardId, string name, string description)
        {
            var template = await _dashboardManager.SaveDashboardTemplateAsync(
                dashboardId,
                name,
                description
            );
            
            var dashboard = await _dashboardRepository.GetAsync(template.DashboardId);
            
            return new DashboardTemplateDto
            {
                Id = template.Id,
                Name = template.Name,
                Description = template.Description,
                DashboardId = template.DashboardId,
                DashboardName = dashboard.Name,
                CreationTime = template.CreationTime,
                CreatorId = template.CreatorId,
                LastModificationTime = template.LastModificationTime,
                LastModifierId = template.LastModifierId
            };
        }
        
        public async Task<List<DashboardTemplateDto>> GetTemplatesAsync()
        {
            var templates = await _dashboardTemplateRepository.GetListAsync();
            var dashboardIds = templates.Select(t => t.DashboardId).Distinct().ToList();
            var dashboards = await _dashboardRepository.GetListAsync(d => dashboardIds.Contains(d.Id));
            
            var templateDtos = new List<DashboardTemplateDto>();
            foreach (var template in templates)
            {
                var dashboard = dashboards.FirstOrDefault(d => d.Id == template.DashboardId);
                if (dashboard == null)
                {
                    continue;
                }
                
                templateDtos.Add(new DashboardTemplateDto
                {
                    Id = template.Id,
                    Name = template.Name,
                    Description = template.Description,
                    DashboardId = template.DashboardId,
                    DashboardName = dashboard.Name,
                    CreationTime = template.CreationTime,
                    CreatorId = template.CreatorId,
                    LastModificationTime = template.LastModificationTime,
                    LastModifierId = template.LastModifierId
                });
            }
            
            return templateDtos;
        }
        
        [Authorize(AnalyticsPermissions.Dashboards.Create)]
        public async Task<DashboardDto> CreateFromTemplateAsync(Guid templateId, string name)
        {
            var template = await _dashboardTemplateRepository.GetAsync(templateId);
            var sourceDashboard = await _dashboardRepository.GetAsync(template.DashboardId);
            
            // Create new dashboard based on template
            var newDashboard = await _dashboardManager.CreateDashboardAsync(
                name,
                sourceDashboard.Description,
                false,
                new DashboardLayoutSetting(
                    sourceDashboard.LayoutSetting.Columns,
                    sourceDashboard.LayoutSetting.Theme,
                    sourceDashboard.LayoutSetting.AutoRefresh,
                    sourceDashboard.LayoutSetting.RefreshIntervalSeconds,
                    sourceDashboard.LayoutSetting.CustomCss,
                    sourceDashboard.LayoutSetting.CompactView
                ),
                CurrentTenant.Id
            );
            
            // Copy widgets
            foreach (var sourceWidget in sourceDashboard.Widgets)
            {
                await _dashboardManager.AddWidgetToDashboardAsync(
                    newDashboard.Id,
                    sourceWidget.Title,
                    sourceWidget.Type,
                    sourceWidget.Settings,
                    sourceWidget.PositionX,
                    sourceWidget.PositionY,
                    sourceWidget.Width,
                    sourceWidget.Height,
                    sourceWidget.DataSourceId
                );
            }
            
            return await MapDashboardToDtoAsync(newDashboard);
        }
        
        private async Task<DashboardDto> MapDashboardToDtoAsync(Dashboard dashboard)
        {
            var dataSourceIds = dashboard.Widgets
                .Where(w => w.DataSourceId.HasValue)
                .Select(w => w.DataSourceId.Value)
                .Distinct()
                .ToList();
            
            var dataSources = await _dataSourceRepository.GetListAsync(ds => dataSourceIds.Contains(ds.Id));
            
            var widgetDtos = new List<WidgetInstanceDto>();
            foreach (var widget in dashboard.Widgets)
            {
                var dataSourceName = "";
                if (widget.DataSourceId.HasValue)
                {
                    var dataSource = dataSources.FirstOrDefault(ds => ds.Id == widget.DataSourceId.Value);
                    if (dataSource != null)
                    {
                        dataSourceName = dataSource.Name;
                    }
                }
                
                widgetDtos.Add(new WidgetInstanceDto
                {
                    Id = widget.Id,
                    Title = widget.Title,
                    Type = widget.Type,
                    DataSourceId = widget.DataSourceId,
                    DataSourceName = dataSourceName,
                    Settings = MapWidgetSettingsToDto(widget.Settings),
                    PositionX = widget.PositionX,
                    PositionY = widget.PositionY,
                    Width = widget.Width,
                    Height = widget.Height
                });
            }
            
            return new DashboardDto
            {
                Id = dashboard.Id,
                Name = dashboard.Name,
                Description = dashboard.Description,
                IsDefault = dashboard.IsDefault,
                LayoutSetting = new DashboardLayoutSettingDto
                {
                    Columns = dashboard.LayoutSetting.Columns,
                    Theme = dashboard.LayoutSetting.Theme,
                    AutoRefresh = dashboard.LayoutSetting.AutoRefresh,
                    RefreshIntervalSeconds = dashboard.LayoutSetting.RefreshIntervalSeconds,
                    CustomCss = dashboard.LayoutSetting.CustomCss,
                    CompactView = dashboard.LayoutSetting.CompactView
                },
                Widgets = widgetDtos,
                CreationTime = dashboard.CreationTime,
                CreatorId = dashboard.CreatorId,
                LastModificationTime = dashboard.LastModificationTime,
                LastModifierId = dashboard.LastModifierId
            };
        }
        
        private WidgetSettingsDto MapWidgetSettingsToDto(ValueObjects.WidgetSettings settings)
        {
            return new WidgetSettingsDto
            {
                ChartType = settings.ChartType,
                DataQuery = settings.DataQuery,
                ShowLegend = settings.ShowLegend,
                ShowLabels = settings.ShowLabels,
                ColorScheme = settings.ColorScheme,
                DateFormat = settings.DateFormat,
                NumberFormat = settings.NumberFormat,
                AutoRefresh = settings.AutoRefresh,
                RefreshIntervalSeconds = settings.RefreshIntervalSeconds,
                AdditionalSettings = settings.AdditionalSettings
            };
        }
    }
}
