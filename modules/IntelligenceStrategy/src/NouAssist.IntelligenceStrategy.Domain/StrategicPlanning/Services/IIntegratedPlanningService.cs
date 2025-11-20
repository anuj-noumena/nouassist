namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Services
{
    /// <summary>
    /// Domain service interface for integrated planning operations
    /// </summary>
    public interface IIntegratedPlanningService
    {
        Task<IntegratedHorizonPlan> CreateIntegratedPlanAsync(
            string planName,
            string description,
            DateTime startDate,
            DateTime endDate,
            PlanningHorizon primaryHorizon,
            string visionStatement);

        Task<List<TimelineEventMarker>> GenerateBackwardPlanAsync(Guid planId);
        
        Task AlignPlanWithStrategicGoalsAsync(Guid planId, List<Guid> goalIds);
    }
}
