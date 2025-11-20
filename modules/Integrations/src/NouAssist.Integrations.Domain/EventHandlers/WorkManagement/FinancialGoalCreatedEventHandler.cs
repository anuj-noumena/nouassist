using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using Task = System.Threading.Tasks.Task;

namespace NouAssist.Integrations.EventHandlers.WorkManagement
{
    /// <summary>
    /// Handles FinancialGoalCreatedEto to create action planning tasks
    /// </summary>
    public class FinancialGoalCreatedEventHandler :
        ITransientDependency
    {
        private readonly FinancialTaskAutomationService _automationService;
        private readonly ILogger<FinancialGoalCreatedEventHandler> _logger;

        // Default IDs - these would ideally come from configuration or a service
        private readonly Guid _defaultTaskTypeId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        private readonly Guid _defaultWorkflowId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        private readonly Guid _todoStatusId = Guid.Parse("00000000-0000-0000-0000-000000000003");

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
                List<TaskInstance> goalTasks = await _automationService.CreateFinancialGoalTasksAsync(
                    goalId,
                    goalName,
                    goalType,
                    targetAmount.Amount,
                    targetAmount.Currency,
                    targetDate,
                    userId,
                    _defaultTaskTypeId,
                    _defaultWorkflowId,
                    _todoStatusId,
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
