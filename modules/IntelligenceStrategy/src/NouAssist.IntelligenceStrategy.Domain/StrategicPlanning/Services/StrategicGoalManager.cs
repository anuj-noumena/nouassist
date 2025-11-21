using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Exceptions;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Repositories;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Specifications;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Services
{
    /// <summary>
    /// Domain service for strategic goal management
    /// </summary>
    public class StrategicGoalManager : DomainService, IStrategicGoalManager
    {
        private readonly IStrategicGoalRepository _strategicGoalRepository;
        private readonly ILogger<StrategicGoalManager> _logger;

        public StrategicGoalManager(
            IStrategicGoalRepository strategicGoalRepository,
            ILogger<StrategicGoalManager> logger)
        {
            _strategicGoalRepository = strategicGoalRepository;
            _logger = logger;
        }

        public async Task<StrategicGoalDefinition> CreateAsync(
            string title,
            string description,
            GoalPriority priority,
            PlanningHorizon horizon,
            DateTime targetDate,
            Guid? ownerId = null)
        {
            try
            {
                // Check for duplicate titles
                if (!await IsTitleUniqueAsync(title))
                {
                    throw new DuplicateGoalTitleException(title);
                }

                var goal = StrategicGoalDefinition.Create(
                    GuidGenerator.Create(),
                    title,
                    description,
                    priority,
                    horizon,
                    targetDate,
                    ownerId,
                    CurrentTenant.Id);

                return await _strategicGoalRepository.InsertAsync(goal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create strategic goal with title {Title}", title);
                throw;
            }
        }

        public async Task<bool> IsTitleUniqueAsync(string title, Guid? existingGoalId = null)
        {
            var specification = new GoalByTitleSpecification(title);
            var existingGoal = await _strategicGoalRepository.FindAsync(specification.ToExpression());
            
            return existingGoal == null || (existingGoalId.HasValue && existingGoal.Id == existingGoalId.Value);
        }

        public async Task ValidateGoalTargetsAsync(StrategicGoalDefinition goal)
        {
            if (!goal.Targets.Any())
            {
                throw new BusinessException("Strategic goal must have at least one target");
            }

            var duplicateMetrics = goal.Targets
                .GroupBy(t => t.Metric.Name)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key);

            if (duplicateMetrics.Any())
            {
                throw new BusinessException($"Duplicate metric names found: {string.Join(", ", duplicateMetrics)}");
            }
        }

        public async Task<List<StrategicGoalDefinition>> GetGoalsRequiringAttentionAsync()
        {
            var behindScheduleSpec = new GoalsBehindScheduleSpecification();
            var duesSoonSpec = new GoalsDueWithinTimeframeSpecification(DateTime.UtcNow, DateTime.UtcNow.AddDays(30));
            var combinedSpec = behindScheduleSpec.Or(duesSoonSpec);

            return await _strategicGoalRepository.GetListAsync(combinedSpec.ToExpression());
        }

        private class GoalByTitleSpecification : StrategicGoalSpecification
        {
            private readonly string _title;

            public GoalByTitleSpecification(string title)
            {
                _title = title;
            }

            public override Expression<Func<StrategicGoalDefinition, bool>> ToExpression()
            {
                return goal => goal.Description.Title == _title;
            }
        }
    }
}
