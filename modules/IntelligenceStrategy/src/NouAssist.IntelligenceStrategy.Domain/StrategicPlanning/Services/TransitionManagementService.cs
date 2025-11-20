namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Services
{
    /// <summary>
    /// Domain service for transition management operations
    /// </summary>
    public class TransitionManagementService : DomainService, ITransitionManagementService
    {
        private readonly ITransitionPlanRepository _transitionPlanRepository;
        private readonly ILogger<TransitionManagementService> _logger;

        public TransitionManagementService(
            ITransitionPlanRepository transitionPlanRepository,
            ILogger<TransitionManagementService> logger)
        {
            _transitionPlanRepository = transitionPlanRepository;
            _logger = logger;
        }

        public async Task<TransitionPlan> CreateTransitionPlanAsync(
            string planName,
            string description,
            TransitionType type,
            DateTime plannedStartDate,
            DateTime estimatedCompletionDate,
            decimal estimatedCost = 0)
        {
            try
            {
                var plan = TransitionPlan.Create(
                    GuidGenerator.Create(),
                    planName,
                    description,
                    type,
                    plannedStartDate,
                    estimatedCompletionDate,
                    estimatedCost,
                    CurrentTenant.Id);

                return await _transitionPlanRepository.InsertAsync(plan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create transition plan with name {PlanName}", planName);
                throw;
            }
        }

        public async Task<decimal> CalculateTransitionReadinessAsync(Guid transitionPlanId)
        {
            var plan = await _transitionPlanRepository.GetPlanWithCapabilityRoadmapAsync(transitionPlanId);
            if (plan == null)
                throw new EntityNotFoundException(typeof(TransitionPlan), transitionPlanId);

            if (!plan.CapabilityRoadmap.Any())
                return 0;

            // Calculate readiness based on capability completion
            var completedCapabilities = plan.CapabilityRoadmap.Count(c => c.IsCompleted);
            var readinessPercentage = (decimal)completedCapabilities / plan.CapabilityRoadmap.Count * 100;

            return readinessPercentage;
        }

        public async Task<List<TransitionPlan>> GetPlansRequiringAttentionAsync()
        {
            return await _transitionPlanRepository.GetPlansRequiringAttentionAsync();
        }
    }
}
