using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Dashboards.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Dashboards
{
    /// <summary>
    /// Application service for dashboard management
    /// </summary>
    public interface IDashboardAppService : IApplicationService
    {
        /// <summary>
        /// Gets a dashboard by ID
        /// </summary>
        Task<DashboardDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a paginated list of dashboards
        /// </summary>
        Task<PagedResultDto<DashboardDto>> GetListAsync(GetDashboardListDto input);
        
        /// <summary>
        /// Creates a new dashboard
        /// </summary>
        Task<DashboardDto> CreateAsync(CreateDashboardDto input);
        
        /// <summary>
        /// Updates an existing dashboard
        /// </summary>
        Task<DashboardDto> UpdateAsync(Guid id, UpdateDashboardDto input);
        
        /// <summary>
        /// Deletes a dashboard
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Configures the layout of a dashboard
        /// </summary>
        Task<DashboardDto> ConfigureLayoutAsync(Guid id, ConfigureDashboardLayoutDto input);
        
        /// <summary>
        /// Adds a widget to a dashboard
        /// </summary>
        Task<WidgetInstanceDto> AddWidgetAsync(Guid dashboardId, AddWidgetToDashboardDto input);
        
        /// <summary>
        /// Updates a widget on a dashboard
        /// </summary>
        Task<WidgetInstanceDto> UpdateWidgetAsync(Guid dashboardId, Guid widgetId, UpdateWidgetDto input);
        
        /// <summary>
        /// Removes a widget from a dashboard
        /// </summary>
        Task DeleteWidgetAsync(Guid dashboardId, Guid widgetId);
    }
}
