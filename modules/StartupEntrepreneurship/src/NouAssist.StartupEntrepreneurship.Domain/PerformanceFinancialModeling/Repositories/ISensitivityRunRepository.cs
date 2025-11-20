namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Repositories
{
    /// <summary>
    /// Repository for SensitivityRun entity
    /// </summary>
    public interface ISensitivityRunRepository : IRepository<SensitivityRun, Guid>
    {
        Task<List<SensitivityRun>> GetByModelAsync(Guid modelId, CancellationToken cancellationToken = default);
        Task<List<SensitivityRun>> GetByScenarioAsync(Guid scenarioId, CancellationToken cancellationToken = default);
        Task<List<SensitivityRun>> GetRecentRunsAsync(int days = 30, CancellationToken cancellationToken = default);
    }
}
