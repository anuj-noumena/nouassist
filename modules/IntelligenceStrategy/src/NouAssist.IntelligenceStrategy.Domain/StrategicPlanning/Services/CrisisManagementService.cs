namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Services
{
    /// <summary>
    /// Domain service for crisis management operations
    /// </summary>
    public class CrisisManagementService : DomainService, ICrisisManagementService
    {
        private readonly ICrisisManagementPlanRepository _crisisManagementPlanRepository;
        private readonly ILogger<CrisisManagementService> _logger;

        public CrisisManagementService(
            ICrisisManagementPlanRepository crisisManagementPlanRepository,
            ILogger<CrisisManagementService> logger)
        {
            _crisisManagementPlanRepository = crisisManagementPlanRepository;
            _logger = logger;
        }

        public async Task<CrisisManagementPlan> CreatePlanAsync(
            string name,
            string description,
            CrisisLevel maxCrisisLevel,
            DateTime nextReviewDate)
        {
            try
            {
                var plan = CrisisManagementPlan.Create(
                    GuidGenerator.Create(),
                    name,
                    description,
                    maxCrisisLevel,
                    nextReviewDate,
                    CurrentTenant.Id);

                return await _crisisManagementPlanRepository.InsertAsync(plan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create crisis management plan with name {Name}", name);
                throw;
            }
        }

        public async Task ActivateCrisisPlanAsync(Guid planId, Guid activatedBy, string reason = null)
        {
            // Check if there's already an active crisis plan
            if (await HasActiveCrisisPlanAsync())
            {
                throw new BusinessException("StrategicPlanning:ErrorMessage:CrisisAlreadyActive");
            }

            var plan = await _crisisManagementPlanRepository.GetAsync(planId);
            plan.Activate(activatedBy, Clock.Now, reason);

            await _crisisManagementPlanRepository.UpdateAsync(plan);
            
            _logger.LogWarning("Crisis management plan {PlanId} activated by user {UserId}. Reason: {Reason}", 
                planId, activatedBy, reason ?? "Not specified");
        }

        public async Task<List<CrisisManagementPlan>> GetPlansForCrisisLevelAsync(CrisisLevel crisisLevel)
        {
            var specification = new CrisisPlansByCrisisLevelSpecification(crisisLevel);
            return await _crisisManagementPlanRepository.GetListAsync(specification.ToExpression());
        }

        public async Task<bool> HasActiveCrisisPlanAsync()
        {
            var specification = new ActiveCrisisPlansSpecification();
            return await _crisisManagementPlanRepository.AnyAsync(specification.ToExpression());
        }
    }
}
