namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Repositories
{
    /// <summary>
    /// Repository for MetricDefinition entity
    /// </summary>
    public interface IMetricDefinitionRepository : IRepository<MetricDefinition, Guid>
    {
        Task<MetricDefinition> FindByNameAsync(string name, Guid? tenantId = null, CancellationToken cancellationToken = default);
        Task<List<MetricDefinition>> GetByTypeAsync(MetricType type, Guid? tenantId = null, CancellationToken cancellationToken = default);
        Task<List<MetricDefinition>> GetActiveDefinitionsAsync(Guid? tenantId = null, CancellationToken cancellationToken = default);
        Task<List<MetricDefinition>> GetByCategoryAsync(Guid categoryId, Guid? tenantId = null, CancellationToken cancellationToken = default);
    }
}
