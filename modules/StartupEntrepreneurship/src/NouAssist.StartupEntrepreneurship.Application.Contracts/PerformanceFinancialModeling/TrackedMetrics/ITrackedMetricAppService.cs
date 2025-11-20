using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.TrackedMetrics.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.TrackedMetrics
{
    /// <summary>
    /// Application service for TrackedMetric management
    /// </summary>
    public interface ITrackedMetricAppService : IApplicationService
    {
        // CRUD operations
        Task<TrackedMetricDto> GetAsync(Guid id);
        Task<TrackedMetricDetailDto> GetDetailAsync(Guid id);
        Task<PagedResultDto<TrackedMetricDto>> GetListAsync(GetTrackedMetricListDto input);
        Task<TrackedMetricDto> CreateAsync(CreateTrackedMetricDto input);
        Task<TrackedMetricDto> UpdateAsync(Guid id, UpdateTrackedMetricDto input);
        Task DeleteAsync(Guid id);

        // Status management
        Task<TrackedMetricDto> ActivateAsync(Guid id);
        Task<TrackedMetricDto> DeactivateAsync(Guid id);

        // Data point management
        Task AddDataPointAsync(Guid metricId, AddDataPointDto input);
        Task BulkAddDataPointsAsync(Guid metricId, BulkAddDataPointsDto input);
        Task<List<MetricDataPointDto>> GetDataPointsAsync(Guid metricId, GetDataPointsDto input);

        // Goal management
        Task<MetricGoalDto> SetGoalAsync(Guid metricId, SetMetricGoalDto input);
        Task<MetricGoalDto> UpdateGoalAsync(Guid goalId, UpdateMetricGoalDto input);
        Task DeactivateGoalAsync(Guid goalId);
        Task<List<MetricGoalDto>> GetGoalsAsync(Guid metricId);

        // Analytics
        Task<decimal?> GetTrendAsync(Guid metricId, int periodDays = 30);
        Task<MetricValueDto> GetAverageValueAsync(Guid metricId, int periodDays = 30);

        // Lookup
        Task<List<TrackedMetricDto>> GetByTypeAsync(MetricType type);
        Task<List<TrackedMetricDto>> GetActiveMetricsAsync();
        Task<List<TrackedMetricDto>> GetMetricsDueForUpdateAsync();
    }
}
