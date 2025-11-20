using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Analytics.DTOs.KPI;
using NouAssist.Analytics.Enums;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Analytics.Services
{
    /// <summary>
    /// Application service interface for KPI operations
    /// </summary>
    public interface IKpiAppService : IApplicationService
    {
        Task<KpiDefinitionDto> GetAsync(Guid id);
        
        Task<PagedResultDto<KpiDefinitionDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        
        Task<List<KpiDefinitionDto>> GetByStatusAsync(KpiStatus status);
        
        Task<KpiDefinitionDto> CreateAsync(CreateKpiDefinitionDto input);
        
        Task<KpiDefinitionDto> UpdateAsync(Guid id, UpdateKpiDefinitionDto input);
        
        Task DeleteAsync(Guid id);
        
        Task<KpiThresholdDto> AddThresholdAsync(Guid id, AddKpiThresholdDto input);
        
        Task RemoveThresholdAsync(Guid id, Guid thresholdId);
        
        Task AddDataSourceAsync(Guid id, Guid dataSourceId);
        
        Task RemoveDataSourceAsync(Guid id, Guid dataSourceId);
        
        Task UpdateValueAsync(Guid id, UpdateKpiValueDto input);
        
        Task<(decimal Value, KpiStatus Status, DateTime LastUpdated)> GetCurrentValueAsync(Guid id);
        
        Task<List<KpiAlertLogDto>> GetRecentAlertsAsync(Guid id, int maxCount = 10);
    }
}
