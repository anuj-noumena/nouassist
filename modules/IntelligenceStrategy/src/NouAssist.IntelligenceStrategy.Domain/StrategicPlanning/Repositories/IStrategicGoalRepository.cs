namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Repositories
{
    /// <summary>
    /// Repository interface for Strategic Goal Definition aggregate
    /// </summary>
    public interface IStrategicGoalRepository : IRepository<StrategicGoalDefinition, Guid>
    {
        Task<List<StrategicGoalDefinition>> GetListAsync(
            Expression<Func<StrategicGoalDefinition, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<int> CountAsync(
            Expression<Func<StrategicGoalDefinition, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(
            Expression<Func<StrategicGoalDefinition, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<StrategicGoalDefinition> FindAsync(
            Expression<Func<StrategicGoalDefinition, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<List<StrategicGoalDefinition>> GetGoalsByOwnerAsync(
            Guid ownerId,
            CancellationToken cancellationToken = default);

        Task<List<StrategicGoalDefinition>> GetGoalsByHorizonAsync(
            PlanningHorizon horizon,
            CancellationToken cancellationToken = default);
    }
}
