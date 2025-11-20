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
    /// Handles BudgetVarianceDetectedEto to create investigation tasks
    /// </summary>
    public class BudgetVarianceDetectedEventHandler :
        IDistributedEventHandler<dynamic>,
        ITransientDependency
    {
        private readonly FinancialTaskAutomationService _automationService;
        private readonly ILogger<BudgetVarianceDetectedEventHandler> _logger;

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
                var investigationTask = await _automationService.CreateBudgetVarianceTaskAsync(
                    budgetId,
                    budgetName,
                    categoryId,
                    categoryName,
                    plannedAmount.Amount,
                    actualAmount.Amount,
                    variancePercent,
                    isOverBudget,
                    userId,
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
