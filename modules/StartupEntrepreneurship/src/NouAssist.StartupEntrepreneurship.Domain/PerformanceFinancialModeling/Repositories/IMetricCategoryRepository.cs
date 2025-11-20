namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Repositories
{
    /// <summary>
    /// Repository for MetricCategory entity
    /// </summary>
    public interface IMetricCategoryRepository : IRepository<MetricCategory, Guid>
    {
        Task<MetricCategory> FindByNameAsync(string name, Guid? tenantId = null, CancellationToken cancellationToken = default);
        Task<List<MetricCategory>> GetActiveCategoriesAsync(Guid? tenantId = null, CancellationToken cancellationToken = default);
        Task<List<MetricCategory>> GetOrderedCategoriesAsync(Guid? tenantId = null, CancellationToken cancellationToken = default);
    }
}
