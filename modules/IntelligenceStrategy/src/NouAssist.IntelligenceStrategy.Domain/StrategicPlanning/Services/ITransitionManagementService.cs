namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Services
{
    /// <summary>
    /// Domain service interface for transition management
    /// </summary>
    public interface ITransitionManagementService
    {
        Task<TransitionPlan> CreateTransitionPlanAsync(
            string planName,
            string description,
            TransitionType type,
            DateTime plannedStartDate,
            DateTime estimatedCompletionDate,
            decimal estimatedCost = 0);

        Task<decimal> CalculateTransitionReadinessAsync(Guid transitionPlanId);
        
        Task<List<TransitionPlan>> GetPlansRequiringAttentionAsync();
    }
}
