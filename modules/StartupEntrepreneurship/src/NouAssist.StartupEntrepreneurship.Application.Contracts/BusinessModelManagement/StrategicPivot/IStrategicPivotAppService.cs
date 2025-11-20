using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.StrategicPivot.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.StrategicPivot
{
    public interface IStrategicPivotAppService : IApplicationService
    {
        Task<StrategicPivotDto> GetAsync(Guid id);
        Task<PagedResultDto<StrategicPivotDto>> GetListAsync(GetStrategicPivotListDto input);
        Task<StrategicPivotDto> CreateAsync(CreateStrategicPivotDto input);
        Task<StrategicPivotDto> UpdateAsync(Guid id, UpdateStrategicPivotDto input);
        Task DeleteAsync(Guid id);

        // Pivot Lifecycle Management
        Task<StrategicPivotDto> StartImplementationAsync(Guid id);
        Task<StrategicPivotDto> CompleteAsync(Guid id, CompletePivotDto input);
        Task<StrategicPivotDto> PutOnHoldAsync(Guid id);
        Task<StrategicPivotDto> ResumeAsync(Guid id);
        Task<StrategicPivotDto> AbandonAsync(Guid id);

        // Impact Metric Management
        Task<ImpactMetricDefinitionDto> CreateImpactMetricAsync(Guid pivotId, CreateImpactMetricDto input);
        Task DeleteImpactMetricAsync(Guid pivotId, Guid metricId);
        Task<List<ImpactMetricDefinitionDto>> GetImpactMetricsAsync(Guid pivotId);
        Task RecordMeasurementAsync(Guid pivotId, Guid metricId, RecordMeasurementDto input);
        Task<List<ImpactValueDto>> GetTrendDataAsync(Guid pivotId, Guid metricId, DateTime fromDate, DateTime toDate);

        // Analytics and Reporting
        Task<PivotAnalysisDto> AnalyzeImpactAsync(Guid id);
        Task<List<StrategicPivotDto>> GetOverduePivotsAsync();
        Task<List<StrategicPivotDto>> GetActivePivotsAsync();
        Task<List<ImpactMetricDefinitionDto>> GetSuggestedMetricsAsync(Guid id);
    }
}
