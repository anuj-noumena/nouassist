using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.WorkManagement.TaskManagement.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.WorkManagement.TaskManagement.EventHandlers
{
    /// <summary>
    /// Handles FinancialGoalCreatedEto to create action planning tasks
    /// </summary>
    public class FinancialGoalCreatedEventHandler :
        IDistributedEventHandler<dynamic>,
        ITransientDependency
    {
        private readonly FinancialTaskAutomationService _automationService;
        private readonly ILogger<FinancialGoalCreatedEventHandler> _logger;

        public FinancialGoalCreatedEventHandler(
            FinancialTaskAutomationService automationService,
            ILogger<FinancialGoalCreatedEventHandler> logger)
        {
            _automationService = automationService;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(dynamic eventData)
        {
            try
            {
                var goalId = (Guid)eventData.GoalId;
                var goalName = (string)eventData.GoalName;
                var goalType = eventData.GoalType.ToString();
                var targetAmount = eventData.TargetAmount;
                var targetDate = (DateTime)eventData.TargetDate;
                var userId = (Guid)eventData.UserId;
                var tenantId = eventData.TenantId as Guid?;

                _logger.LogInformation(
                    "Financial goal created: {GoalName}, creating action planning tasks",
                    goalName);

                // Create multiple tasks for goal achievement
                var goalTasks = await _automationService.CreateFinancialGoalTasksAsync(
                    goalId,
                    goalName,
                    goalType,
                    targetAmount.Amount,
                    targetAmount.Currency,
                    targetDate,
                    userId,
                    tenantId);

                _logger.LogInformation(
                    "Created {Count} action tasks for goal {GoalName}",
                    goalTasks.Count, goalName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling FinancialGoalCreatedEto");
                throw;
            }
        }
    }
}
