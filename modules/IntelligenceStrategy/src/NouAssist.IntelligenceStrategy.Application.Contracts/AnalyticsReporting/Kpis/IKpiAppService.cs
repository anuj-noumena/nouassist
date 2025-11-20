using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Kpis.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Kpis
{
    /// <summary>
    /// Application service for KPI management
    /// </summary>
    public interface IKpiAppService : IApplicationService
    {
        /// <summary>
        /// Gets a KPI by ID
        /// </summary>
        Task<KpiDefinitionDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a paginated list of KPIs
        /// </summary>
        Task<PagedResultDto<KpiDefinitionDto>> GetListAsync(GetKpiListDto input);
        
        /// <summary>
        /// Creates a new KPI
        /// </summary>
        Task<KpiDefinitionDto> CreateAsync(CreateKpiDefinitionDto input);
        
        /// <summary>
        /// Updates an existing KPI
        /// </summary>
        Task<KpiDefinitionDto> UpdateAsync(Guid id, UpdateKpiDefinitionDto input);
        
        /// <summary>
        /// Deletes a KPI
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Adds a threshold to a KPI
        /// </summary>
        Task<KpiThresholdDto> AddThresholdAsync(Guid kpiId, AddKpiThresholdDto input);
        
        /// <summary>
        /// Updates a threshold on a KPI
        /// </summary>
        Task<KpiThresholdDto> UpdateThresholdAsync(Guid kpiId, Guid thresholdId, UpdateKpiThresholdDto input);
        
        /// <summary>
        /// Removes a threshold from a KPI
        /// </summary>
        Task DeleteThresholdAsync(Guid kpiId, Guid thresholdId);
        
        /// <summary>
        /// Updates the value of a KPI
        /// </summary>
        Task<KpiValueDto> UpdateValueAsync(Guid kpiId, UpdateKpiValueDto input);
        
        /// <summary>
        /// Gets KPI alerts for a KPI
        /// </summary>
        Task<PagedResultDto<KpiAlertLogDto>> GetAlertsAsync(Guid kpiId, GetKpiAlertListDto input);
        
        /// <summary>
        /// Gets KPI alerts across all KPIs
        /// </summary>
        Task<PagedResultDto<KpiAlertLogDto>> GetAllAlertsAsync(GetKpiAlertListDto input);
    }
}
