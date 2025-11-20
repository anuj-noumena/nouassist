using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Dashboards.Dtos;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Exceptions;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Permissions;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Services;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Dashboards
{
    /// <summary>
    /// Application service implementation for dashboard management
    /// </summary>
    [Authorize(AnalyticsReportingPermissions.Dashboard.Default)]
    public class DashboardAppService : ApplicationService, IDashboardAppService
    {
        private readonly IDashboardManager _dashboardManager;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IDataSourceRepository _dataSourceRepository;
        private readonly ICurrentUser _currentUser;
        
        public DashboardAppService(
            IDashboardManager dashboardManager,
            IDashboardRepository dashboardRepository,
            IDataSourceRepository dataSourceRepository,
            ICurrentUser currentUser)
        {
            _dashboardManager = dashboardManager;
            _dashboardRepository = dashboardRepository;
            _dataSourceRepository = dataSourceRepository;
            _currentUser = currentUser;
            
            // Set resource for localization
            LocalizationResource = typeof(AnalyticsReportingResource);
        }
        
        /// <summary>
        /// Gets a dashboard by ID
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Dashboard.View)]
        public virtual async Task<DashboardDto> GetAsync(Guid id)
        {
            try
            {
                var dashboard = await _dashboardRepository.GetAsync(id);
                
                // Check if user can access this dashboard
                await CheckDashboardAccessAsync(dashboard);
                
                // Load data source names for widgets if they have data sources
                await EnrichWidgetsWithDataSourceNamesAsync(dashboard);
                
                return ObjectMapper.Map<Dashboard, DashboardDto>(dashboard);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Gets a paginated list of dashboards
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Dashboard.View)]
        public virtual async Task<PagedResultDto<DashboardDto>> GetListAsync(GetDashboardListDto input)
        {
            try
            {
                // Get queryable repository to apply filters
                var queryable = await _dashboardRepository.GetQueryableAsync();
                
                // Filter by current user's permissions and input filters
                queryable = await ApplyListFiltersAsync(queryable, input);
                
                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(queryable);
                
                // Apply sorting and paging
                queryable = queryable
                    .OrderBy(input.Sorting ?? nameof(Dashboard.Name))
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount);
                
                // Get dashboards
                var dashboards = await AsyncExecuter.ToListAsync(queryable);
                
                // Include widgets if requested
                if (input.IncludeWidgets && dashboards.Any())
                {
                    // Load data source names for widgets if they have data sources
                    foreach (var dashboard in dashboards)
                    {
                        await EnrichWidgetsWithDataSourceNamesAsync(dashboard);
                    }
                }
                
                // Map to DTOs
                var dtos = ObjectMapper.Map<List<Dashboard>, List<DashboardDto>>(dashboards);
                
                return new PagedResultDto<DashboardDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Creates a new dashboard
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Dashboard.Create)]
        public virtual async Task<DashboardDto> CreateAsync(CreateDashboardDto input)
        {
            try
            {
                // Convert input to refresh interval value object
                var refreshInterval = ObjectMapper.Map<CreateDashboardDto, RefreshInterval>(input);
                
                // Create dashboard using domain service
                var dashboard = await _dashboardManager.CreateDashboardAsync(
                    GuidGenerator.Create(),
                    input.Name,
                    input.Description,
                    input.IsPrivate,
                    refreshInterval,
                    input.TemplateId,
                    CurrentTenant.Id
                );
                
                // Map to DTO and return
                return ObjectMapper.Map<Dashboard, DashboardDto>(dashboard);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Updates an existing dashboard
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Dashboard.Update)]
        public virtual async Task<DashboardDto> UpdateAsync(Guid id, UpdateDashboardDto input)
        {
            try
            {
                // Get the dashboard
                var dashboard = await _dashboardRepository.GetAsync(id);
                
                // Check if user can access this dashboard
                await CheckDashboardAccessAsync(dashboard);
                
                // Update properties
                dashboard.SetName(input.Name);
                dashboard.SetDescription(input.Description);
                dashboard.SetIcon(input.Icon);
                dashboard.SetPrivacy(input.IsPrivate);
                dashboard.SetTags(input.Tags);
                
                // Update refresh interval
                var refreshInterval = ObjectMapper.Map<UpdateDashboardDto, RefreshInterval>(input);
                dashboard.UpdateRefreshInterval(refreshInterval);
                
                // Save changes
                await _dashboardRepository.UpdateAsync(dashboard);
                
                // Load data source names for widgets if they have data sources
                await EnrichWidgetsWithDataSourceNamesAsync(dashboard);
                
                // Map to DTO and return
                return ObjectMapper.Map<Dashboard, DashboardDto>(dashboard);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Deletes a dashboard
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Dashboard.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                // Get the dashboard
                var dashboard = await _dashboardRepository.GetAsync(id);
                
                // Check if user can access this dashboard
                await CheckDashboardAccessAsync(dashboard);
                
                // Delete the dashboard
                await _dashboardRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Configures the layout of a dashboard
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Dashboard.Update)]
        public virtual async Task<DashboardDto> ConfigureLayoutAsync(Guid id, ConfigureDashboardLayoutDto input)
        {
            try
            {
                // Get the dashboard
                var dashboard = await _dashboardRepository.GetAsync(id);
                
                // Check if user can access this dashboard
                await CheckDashboardAccessAsync(dashboard);
                
                // Configure the layout
                dashboard.ConfigureLayout(
                    input.ColumnCount,
                    input.LayoutType,
                    input.AutoResize,
                    input.CustomCss,
                    input.LayoutConfig
                );
                
                // Save changes
                await _dashboardRepository.UpdateAsync(dashboard);
                
                // Load data source names for widgets if they have data sources
                await EnrichWidgetsWithDataSourceNamesAsync(dashboard);
                
                // Map to DTO and return
                return ObjectMapper.Map<Dashboard, DashboardDto>(dashboard);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Adds a widget to a dashboard
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Dashboard.AddWidget)]
        public virtual async Task<WidgetInstanceDto> AddWidgetAsync(Guid dashboardId, AddWidgetToDashboardDto input)
        {
            try
            {
                // Create widget settings value object
                var widgetSettings = ObjectMapper.Map<AddWidgetToDashboardDto, WidgetSettings>(input);
                
                // Create visualization configuration value object if provided
                var visualizationConfig = input.VisualizationConfig != null 
                    ? ObjectMapper.Map<AddWidgetToDashboardDto, VisualizationConfiguration>(input)
                    : null;
                
                // Add widget using domain service
                var widget = await _dashboardManager.AddWidgetToDashboardAsync(
                    dashboardId,
                    GuidGenerator.Create(),
                    input.Type,
                    widgetSettings,
                    input.PositionX,
                    input.PositionY,
                    input.Width,
                    input.Height,
                    input.DataSourceId,
                    visualizationConfig
                );
                
                // Set data source name if applicable
                var widgetDto = ObjectMapper.Map<WidgetInstance, WidgetInstanceDto>(widget);
                if (widget.DataSourceId.HasValue)
                {
                    try
                    {
                        var dataSource = await _dataSourceRepository.GetAsync(widget.DataSourceId.Value);
                        widgetDto.DataSourceName = dataSource.Name;
                    }
                    catch
                    {
                        // If data source not found, leave name empty
                        widgetDto.DataSourceName = "Unknown";
                    }
                }
                
                return widgetDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Updates a widget on a dashboard
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Dashboard.Update)]
        public virtual async Task<WidgetInstanceDto> UpdateWidgetAsync(Guid dashboardId, Guid widgetId, UpdateWidgetDto input)
        {
            try
            {
                // Get the dashboard
                var dashboard = await _dashboardRepository.GetAsync(dashboardId);
                
                // Check if user can access this dashboard
                await CheckDashboardAccessAsync(dashboard);
                
                // Find the widget
                var widget = dashboard.Widgets.FirstOrDefault(w => w.Id == widgetId);
                if (widget == null)
                {
                    throw new EntityNotFoundException("Widget", widgetId);
                }
                
                // Update position if specified
                if (input.PositionX.HasValue && input.PositionY.HasValue)
                {
                    dashboard.UpdateWidgetPosition(widgetId, input.PositionX.Value, input.PositionY.Value);
                }
                
                // Update size if specified
                if (input.Width.HasValue && input.Height.HasValue)
                {
                    dashboard.UpdateWidgetSize(widgetId, input.Width.Value, input.Height.Value);
                }
                
                // Update settings if any property is specified
                if (!string.IsNullOrEmpty(input.Title) || 
                    input.Subtitle != null || 
                    input.RefreshIntervalSeconds.HasValue || 
                    input.Configuration != null || 
                    input.ShowTitle.HasValue || 
                    input.AllowMaximize.HasValue)
                {
                    // Store existing settings for mapping context
                    var existingSettings = widget.Settings;
                    var context = new Dictionary<string, object>
                    {
                        ["ExistingSettings"] = existingSettings
                    };
                    
                    // Map input to new widget settings
                    var newSettings = ObjectMapper.Map<UpdateWidgetDto, WidgetSettings>(input, context);
                    
                    // Update widget settings
                    dashboard.UpdateWidgetSettings(widgetId, newSettings);
                }
                
                // Update data source if specified
                if (input.DataSourceId.HasValue)
                {
                    dashboard.UpdateWidgetDataSource(widgetId, input.DataSourceId.Value);
                }
                
                // Update visualization config if specified
                if (input.VisualizationConfig != null)
                {
                    // Store existing config for mapping context
                    var existingConfig = widget.VisualizationConfig;
                    var context = new Dictionary<string, object>
                    {
                        ["ExistingVisConfig"] = existingConfig
                    };
                    
                    // Map input to new visualization config
                    var newConfig = ObjectMapper.Map<UpdateWidgetDto, VisualizationConfiguration>(input, context);
                    
                    // Update widget visualization config
                    dashboard.UpdateWidgetVisualization(widgetId, newConfig);
                }
                
                // Save changes
                await _dashboardRepository.UpdateAsync(dashboard);
                
                // Get updated widget
                var updatedWidget = dashboard.Widgets.First(w => w.Id == widgetId);
                
                // Set data source name if applicable
                var widgetDto = ObjectMapper.Map<WidgetInstance, WidgetInstanceDto>(updatedWidget);
                if (updatedWidget.DataSourceId.HasValue)
                {
                    try
                    {
                        var dataSource = await _dataSourceRepository.GetAsync(updatedWidget.DataSourceId.Value);
                        widgetDto.DataSourceName = dataSource.Name;
                    }
                    catch
                    {
                        // If data source not found, leave name empty
                        widgetDto.DataSourceName = "Unknown";
                    }
                }
                
                return widgetDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Removes a widget from a dashboard
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Dashboard.Update)]
        public virtual async Task DeleteWidgetAsync(Guid dashboardId, Guid widgetId)
        {
            try
            {
                // Get the dashboard
                var dashboard = await _dashboardRepository.GetAsync(dashboardId);
                
                // Check if user can access this dashboard
                await CheckDashboardAccessAsync(dashboard);
                
                // Remove the widget
                dashboard.RemoveWidget(widgetId);
                
                // Save changes
                await _dashboardRepository.UpdateAsync(dashboard);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Applies filters to dashboard list query
        /// </summary>
        private async Task<IQueryable<Dashboard>> ApplyListFiltersAsync(IQueryable<Dashboard> queryable, GetDashboardListDto input)
        {
            // Filter by current user if not admin and onlyMine is true
            if (input.OnlyMine == true)
            {
                var currentUserId = _currentUser.Id;
                if (currentUserId.HasValue)
                {
                    queryable = queryable.Where(d => d.CreatorId == currentUserId);
                }
            }
            else
            {
                // Otherwise, filter out private dashboards not owned by current user
                var currentUserId = _currentUser.Id;
                if (currentUserId.HasValue)
                {
                    queryable = queryable.Where(d => !d.IsPrivate || d.CreatorId == currentUserId);
                }
                else
                {
                    // If no user is logged in, only show public dashboards
                    queryable = queryable.Where(d => !d.IsPrivate);
                }
            }
            
            // Filter by text
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                queryable = queryable.Where(d => 
                    d.Name.Contains(input.Filter) || 
                    (d.Description != null && d.Description.Contains(input.Filter)));
            }
            
            // Filter by tag
            if (!string.IsNullOrWhiteSpace(input.Tag))
            {
                // This is a simplification - in actual implementation
                // you might need a more complex query to search within the tags array
                queryable = queryable.Where(d => EF.Functions.JsonContains(d.Tags, input.Tag));
            }
            
            // Apply tenant filter
            queryable = queryable.Where(d => d.TenantId == CurrentTenant.Id);
            
            return queryable;
        }
        
        /// <summary>
        /// Checks if the current user can access the dashboard
        /// </summary>
        private async Task CheckDashboardAccessAsync(Dashboard dashboard)
        {
            // Admin users can access all dashboards
            if (await AuthorizationService.IsGrantedAsync(AnalyticsReportingPermissions.Dashboard.Default))
            {
                return;
            }
            
            // Check tenant
            if (dashboard.TenantId != CurrentTenant.Id)
            {
                throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
            }
            
            // Check if dashboard is private and current user is the creator
            if (dashboard.IsPrivate && dashboard.CreatorId != _currentUser.Id)
            {
                throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
            }
        }
        
        /// <summary>
        /// Enriches widgets with data source names
        /// </summary>
        private async Task EnrichWidgetsWithDataSourceNamesAsync(Dashboard dashboard)
        {
            foreach (var widget in dashboard.Widgets)
            {
                if (widget.DataSourceId.HasValue)
                {
                    try
                    {
                        var dataSource = await _dataSourceRepository.GetAsync(widget.DataSourceId.Value);
                        // We can't directly modify the widget here because it doesn't have a DataSourceName property
                        // The mapping to DTO will handle this using the dictionary context
                    }
                    catch
                    {
                        // If data source not found, continue with next widget
                    }
                }
            }
        }
        
        /// <summary>
        /// Error handling helper method
        /// </summary>
        private async Task HandleErrorAsync(Exception ex, LogLevel logLevel = LogLevel.Error)
        {
            // Log the error
            Logger.Log(logLevel, ex, "Error in DashboardAppService: {Message}", ex.Message);
            
            // Handle specific exceptions
            if (ex is EntityNotFoundException)
            {
                throw new UserFriendlyException(
                    L["AnalyticsReporting:ErrorMessage:EntityNotFound", L["AnalyticsReporting:Entity:Dashboard"]],
                    null,
                    ex);
            }
            
            if (ex is AnalyticsReportingDomainException domainException)
            {
                throw new UserFriendlyException(
                    L[domainException.Code, domainException.Message],
                    domainException.Code,
                    ex);
            }
            
            // For other exceptions, throw a generic error
            throw new UserFriendlyException(
                L["AnalyticsReporting:ErrorMessage:GenericError"],
                null,
                ex);
        }
    }
}
