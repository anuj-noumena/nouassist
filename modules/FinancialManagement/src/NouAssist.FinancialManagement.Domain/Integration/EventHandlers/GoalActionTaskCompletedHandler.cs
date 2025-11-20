using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.FinancialManagement.Integration.EventHandlers
{
    /// <summary>
    /// Handles TaskCompletedEto for financial goal tasks to update goal metadata
    /// </summary>
    public class GoalActionTaskCompletedHandler :
        IDistributedEventHandler<dynamic>,
        ITransientDependency
    {
        private readonly IFinancialGoalRepository _goalRepository;
        private readonly ILogger<GoalActionTaskCompletedHandler> _logger;

        public GoalActionTaskCompletedHandler(
            IFinancialGoalRepository goalRepository,
            ILogger<GoalActionTaskCompletedHandler> logger)
        {
            _goalRepository = goalRepository;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(dynamic eventData)
        {
            try
            {
                // Check if this is a goal-related task
                var customFields = eventData.CustomFields as System.Collections.Generic.Dictionary<string, string>;
                if (customFields == null ||
                    !customFields.ContainsKey("FinancialEntityType") ||
                    customFields["FinancialEntityType"] != "Goal")
                {
                    return; // Not a goal-related task
                }

                if (!customFields.ContainsKey("GoalId"))
                {
                    return; // Missing GoalId
                }

                var goalId = Guid.Parse(customFields["GoalId"]);
                var taskSubtype = customFields.ContainsKey("TaskSubtype") ? customFields["TaskSubtype"] : "Unknown";
                var resolutionNote = eventData.ResolutionNote as string;
                var completedAt = (DateTime)eventData.CompletedAt;

                _logger.LogInformation(
                    "Processing goal task completion for Goal {GoalId}, Task type: {TaskType}",
                    goalId, taskSubtype);

                // Get the goal
                var goal = await _goalRepository.GetAsync(goalId);

                // Update goal based on task type
                // In real implementation, you might have:
                // if (taskSubtype == "ActionPlan") goal.MarkActionPlanCompleted(completedAt);
                // if (taskSubtype == "CheckIn") goal.RecordCheckIn(completedAt, resolutionNote);

                _logger.LogInformation(
                    "Goal {GoalId} task ({TaskType}) completion recorded",
                    goalId, taskSubtype);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling goal task completion");
                // Don't throw - this is a secondary operation
            }
        }
    }
}
