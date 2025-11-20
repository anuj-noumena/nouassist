using System;
using Microsoft.Extensions.Logging;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using Task = System.Threading.Tasks.Task;

namespace NouAssist.Integrations.EventHandlers.WorkManagement
{
    /// <summary>
    /// Handles BudgetVarianceDetectedEto to create investigation tasks
    /// </summary>
    public class BudgetVarianceDetectedEventHandler :
        ITransientDependency
    {
        private readonly FinancialTaskAutomationService _automationService;
        private readonly ILogger<BudgetVarianceDetectedEventHandler> _logger;

        // Default IDs - these would ideally come from configuration or a service
        private readonly Guid _defaultTaskTypeId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        private readonly Guid _defaultWorkflowId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        private readonly Guid _todoStatusId = Guid.Parse("00000000-0000-0000-0000-000000000003");

        public BudgetVarianceDetectedEventHandler(
            FinancialTaskAutomationService automationService,
            ILogger<BudgetVarianceDetectedEventHandler> logger)
        {
            _automationService = automationService;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(dynamic eventData)
        {
            try
            {
                var budgetId = (Guid)eventData.BudgetId;
                var budgetName = (string)eventData.BudgetName;
                var categoryId = (Guid)eventData.CategoryId;
                var categoryName = (string)eventData.CategoryName;
                var plannedAmount = eventData.PlannedAmount;
                var actualAmount = eventData.ActualAmount;
                var variancePercent = (decimal)eventData.VariancePercent;
                var isOverBudget = (bool)eventData.IsOverBudget;
                var userId = (Guid)eventData.UserId;
                var tenantId = eventData.TenantId as Guid?;

                _logger.LogInformation(
                    "Budget variance detected for {BudgetName} - {CategoryName}: {Variance}%",
                    budgetName, categoryName, variancePercent);

                // Create variance investigation task
                TaskInstance investigationTask = await _automationService.CreateBudgetVarianceTaskAsync(
                    budgetId,
                    budgetName,
                    categoryId,
                    categoryName,
                    plannedAmount.Amount,
                    actualAmount.Amount,
                    variancePercent,
                    isOverBudget,
                    userId,
                    _defaultTaskTypeId,
                    _defaultWorkflowId,
                    _todoStatusId,
                    tenantId);

                _logger.LogInformation(
                    "Created investigation task {TaskId} for budget variance in {CategoryName}",
                    investigationTask.Id, categoryName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling BudgetVarianceDetectedEto");
                throw;
            }
        }
    }
}
