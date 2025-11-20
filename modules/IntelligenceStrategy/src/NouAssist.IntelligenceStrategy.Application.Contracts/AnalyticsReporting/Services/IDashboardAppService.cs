using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Analytics.DTOs.Dashboard;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Analytics.Services
{
    /// <summary>
    /// Application service interface for dashboard operations
    /// </summary>
    public interface IDashboardAppService : IApplicationService
    {
        Task<DashboardDto> GetAsync(Guid id);
        
        Task<PagedResultDto<DashboardDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        
        Task<DashboardDto> GetDefaultAsync();
        
        Task<DashboardDto> CreateAsync(CreateDashboardDto input);
        
        Task<DashboardDto> UpdateAsync(Guid id, UpdateDashboardDto input);
        
        Task DeleteAsync(Guid id);
        
        Task<WidgetInstanceDto> AddWidgetAsync(Guid dashboardId, AddWidgetDto input);
        
        Task<WidgetInstanceDto> UpdateWidgetAsync(Guid dashboardId, Guid widgetId, UpdateWidgetDto input);
        
        Task RemoveWidgetAsync(Guid dashboardId, Guid widgetId);
        
        Task<DashboardTemplateDto> SaveAsTemplateAsync(Guid dashboardId, string name, string description);
        
        Task<List<DashboardTemplateDto>> GetTemplatesAsync();
        
        Task<DashboardDto> CreateFromTemplateAsync(Guid templateId, string name);
    }
}
