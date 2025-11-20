namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Repositories
{
    /// <summary>
    /// Repository interface for Sustainable Growth Framework aggregate
    /// </summary>
    public interface ISustainableGrowthFrameworkRepository : IRepository<SustainableGrowthFramework, Guid>
    {
        Task<List<SustainableGrowthFramework>> GetActiveFrameworksAsync(
            CancellationToken cancellationToken = default);

        Task<SustainableGrowthFramework> GetFrameworkWithEsgGoalsAsync(
            Guid frameworkId,
            CancellationToken cancellationToken = default);
    }
}
