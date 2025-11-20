namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Repositories
{
    /// <summary>
    /// Repository interface for Transition Plan aggregate
    /// </summary>
    public interface ITransitionPlanRepository : IRepository<TransitionPlan, Guid>
    {
        Task<List<TransitionPlan>> GetActivePlansAsync(
            CancellationToken cancellationToken = default);

        Task<TransitionPlan> GetPlanWithCapabilityRoadmapAsync(
            Guid planId,
            CancellationToken cancellationToken = default);

        Task<List<TransitionPlan>> GetPlansByTypeAsync(
            TransitionType type,
            CancellationToken cancellationToken = default);

        Task<List<TransitionPlan>> GetPlansRequiringAttentionAsync(
            CancellationToken cancellationToken = default);
    }
}
