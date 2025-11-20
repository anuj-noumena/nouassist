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
    /// Handles GoalCheckInDueEto to create periodic check-in tasks
    /// </summary>
    public class GoalCheckInDueEventHandler :
        IDistributedEventHandler<dynamic>,
        ITransientDependency
    {
        private readonly FinancialTaskAutomationService _automationService;
        private readonly ILogger<GoalCheckInDueEventHandler> _logger;

        public GoalCheckInDueEventHandler(
            FinancialTaskAutomationService automationService,
            ILogger<GoalCheckInDueEventHandler> logger)
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
                var targetAmount = eventData.TargetAmount;
                var currentAmount = eventData.CurrentAmount;
                var progressPercent = (decimal)eventData.ProgressPercent;
                var targetDate = (DateTime)eventData.TargetDate;
                var checkInFrequency = (string)eventData.CheckInFrequency;
                var userId = (Guid)eventData.UserId;
                var tenantId = eventData.TenantId as Guid?;

                _logger.LogInformation(
                    "Goal check-in due for {GoalName} ({Progress}% complete)",
                    goalName, progressPercent);

                // Create check-in task
                var checkInTask = await _automationService.CreateGoalCheckInTaskAsync(
                    goalId,
                    goalName,
                    targetAmount.Amount,
                    currentAmount.Amount,
                    progressPercent,
                    targetAmount.Currency,
                    targetDate,
                    checkInFrequency,
                    userId,
                    tenantId);

                _logger.LogInformation(
                    "Created check-in task {TaskId} for goal {GoalName}",
                    checkInTask.Id, goalName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling GoalCheckInDueEto");
                throw;
            }
        }
    }
}
