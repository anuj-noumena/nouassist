namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Repositories
{
    /// <summary>
    /// Repository interface for Crisis Management Plan aggregate
    /// </summary>
    public interface ICrisisManagementPlanRepository : IRepository<CrisisManagementPlan, Guid>
    {
        Task<List<CrisisManagementPlan>> GetListAsync(
            Expression<Func<CrisisManagementPlan, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(
            Expression<Func<CrisisManagementPlan, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<CrisisManagementPlan> GetActivePlanAsync(
            CancellationToken cancellationToken = default);

        Task<List<CrisisManagementPlan>> GetPlansDueForReviewAsync(
            CancellationToken cancellationToken = default);
    }
}
