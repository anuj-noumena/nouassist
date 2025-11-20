using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.FinancialPlanning.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace NouAssist.FinancialManagement.Integration.EventHandlers
{
    /// <summary>
    /// Handles TaskCompletedEto for budget approval tasks to activate the budget
    /// </summary>
    public class BudgetApprovalTaskCompletedHandler :
        ITransientDependency
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly ILogger<BudgetApprovalTaskCompletedHandler> _logger;

        public BudgetApprovalTaskCompletedHandler(
            IBudgetRepository budgetRepository,
            ILogger<BudgetApprovalTaskCompletedHandler> logger)
        {
            _budgetRepository = budgetRepository;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(dynamic eventData)
        {
            try
            {
                // Check if this is a budget approval task
                var customFields = eventData.CustomFields as System.Collections.Generic.Dictionary<string, string>;
                if (customFields == null ||
                    !customFields.ContainsKey("FinancialEntityType") ||
                    customFields["FinancialEntityType"] != "Budget")
                {
                    return; // Not a budget-related task
                }

                if (!customFields.ContainsKey("BudgetId"))
                {
                    return; // Missing BudgetId
                }

                var budgetId = Guid.Parse(customFields["BudgetId"]);
                var resolutionNote = eventData.ResolutionNote as string;

                _logger.LogInformation(
                    "Processing budget approval task completion for Budget {BudgetId}",
                    budgetId);

                // Get the budget
                var budget = await _budgetRepository.GetAsync(budgetId);

                // Activate the budget
                budget.Activate();

                await _budgetRepository.UpdateAsync(budget, autoSave: true);

                _logger.LogInformation(
                    "Budget {BudgetId} activated via task completion",
                    budgetId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling budget approval task completion");
                // Don't throw - this is a secondary operation
            }
        }
    }
}
