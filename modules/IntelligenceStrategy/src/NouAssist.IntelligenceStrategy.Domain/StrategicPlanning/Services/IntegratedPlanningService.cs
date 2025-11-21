using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Entities;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Repositories;
using Volo.Abp.Domain.Services;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Services
{
    /// <summary>
    /// Domain service for integrated planning operations
    /// </summary>
    public class IntegratedPlanningService : DomainService, IIntegratedPlanningService
    {
        private readonly IIntegratedHorizonPlanRepository _integratedHorizonPlanRepository;
        private readonly IStrategicGoalRepository _strategicGoalRepository;
        private readonly ILogger<IntegratedPlanningService> _logger;

        public IntegratedPlanningService(
            IIntegratedHorizonPlanRepository integratedHorizonPlanRepository,
            IStrategicGoalRepository strategicGoalRepository,
            ILogger<IntegratedPlanningService> logger)
        {
            _integratedHorizonPlanRepository = integratedHorizonPlanRepository;
            _strategicGoalRepository = strategicGoalRepository;
            _logger = logger;
        }

        public async Task<IntegratedHorizonPlan> CreateIntegratedPlanAsync(
            string planName,
            string description,
            DateTime startDate,
            DateTime endDate,
            PlanningHorizon primaryHorizon,
            string visionStatement)
        {
            try
            {
                var plan = IntegratedHorizonPlan.Create(
                    GuidGenerator.Create(),
                    planName,
                    description,
                    startDate,
                    endDate,
                    primaryHorizon,
                    visionStatement,
                    CurrentTenant.Id);

                return await _integratedHorizonPlanRepository.InsertAsync(plan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create integrated horizon plan with name {PlanName}", planName);
                throw;
            }
        }

        public async Task<List<TimelineEventMarker>> GenerateBackwardPlanAsync(Guid planId)
        {
            var plan = await _integratedHorizonPlanRepository.GetPlanWithTimelineEventsAsync(planId);
            if (plan == null)
                throw new EntityNotFoundException(typeof(IntegratedHorizonPlan), planId);

            return plan.GenerateBackwardPlan();
        }

        public async Task AlignPlanWithStrategicGoalsAsync(Guid planId, List<Guid> goalIds)
        {
            var plan = await _integratedHorizonPlanRepository.GetAsync(planId);
            var goals = await _strategicGoalRepository.GetListAsync(g => goalIds.Contains(g.Id));

            foreach (var goal in goals)
            {
                plan.AddTimelineEvent(
                    goal.TargetDate,
                    $"Strategic Goal: {goal.Description.Title}",
                    goal.Description.Description,
                    "Strategic Goal",
                    goal.Priority == GoalPriority.Critical);
            }

            await _integratedHorizonPlanRepository.UpdateAsync(plan);
        }
    }
}
