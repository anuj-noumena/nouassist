namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Repositories
{
    /// <summary>
    /// Repository for TrackedMetric aggregate
    /// </summary>
    public interface ITrackedMetricRepository : IRepository<TrackedMetric, Guid>
    {
        Task<TrackedMetric> FindByNameAsync(string name, Guid? tenantId = null, CancellationToken cancellationToken = default);
        Task<List<TrackedMetric>> GetByTypeAsync(MetricType type, Guid? tenantId = null, CancellationToken cancellationToken = default);
        Task<List<TrackedMetric>> GetActiveMetricsAsync(Guid? tenantId = null, CancellationToken cancellationToken = default);
        Task<List<TrackedMetric>> GetMetricsDueForUpdateAsync(Guid? tenantId = null, CancellationToken cancellationToken = default);
        Task<List<TrackedMetric>> GetMetricsWithActiveGoalsAsync(Guid? tenantId = null, CancellationToken cancellationToken = default);
        Task<List<TrackedMetric>> GetListAsync(TrackedMetricSpecification specification, CancellationToken cancellationToken = default);
        Task<int> CountAsync(TrackedMetricSpecification specification, CancellationToken cancellationToken = default);
    }
}
