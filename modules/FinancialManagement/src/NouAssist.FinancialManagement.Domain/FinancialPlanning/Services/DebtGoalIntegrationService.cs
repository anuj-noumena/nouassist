using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Data;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace NouAssist.FinancialManagement.FinancialPlanning.Services
{
    /// <summary>
    /// Domain service for integrating debt payoff plans with financial goals
    /// </summary>
    public class DebtGoalIntegrationService : DomainService
    {
        private readonly IFinancialGoalRepository _goalRepository;
        private readonly ILogger<DebtGoalIntegrationService> _logger;

        public DebtGoalIntegrationService(
            IFinancialGoalRepository goalRepository,
            ILogger<DebtGoalIntegrationService> logger)
        {
            _goalRepository = goalRepository;
            _logger = logger;
        }

        /// <summary>
        /// Create a financial goal from a debt payoff plan
        /// </summary>
        [UnitOfWork]
        public virtual async Task<FinancialGoal> CreateGoalFromPayoffPlanAsync(
            Guid payoffPlanId,
            string planName,
            string strategy,
            decimal totalDebtAmount,
            decimal monthlyPaymentAmount,
            DateTime targetPayoffDate,
            string currency,
            Guid userId,
            Guid? tenantId = null)
        {
            _logger.LogInformation(
                "Creating financial goal from payoff plan: {PlanName} - {TotalDebt}",
                planName,
                totalDebtAmount);

            var goalName = $"Pay Off All Debts - {planName}";
            var goalDescription = $"Debt payoff using {strategy} strategy. " +
                                  $"Monthly payment: {monthlyPaymentAmount:C} {currency}. " +
                                  $"Total debt: {totalDebtAmount:C} {currency}.";

            var goal = new FinancialGoal(
                id: GuidGenerator.Create(),
                tenantId: tenantId,
                userId: userId,
                name: goalName,
                type: GoalType.DebtPayoff,
                targetAmount: new MonetaryValue(totalDebtAmount, currency),
                currentAmount: MonetaryValue.Zero(currency),
                targetDate: targetPayoffDate,
                description: goalDescription
            );

            // Link to payoff plan (store in notes or custom field)
            goal.UpdateNotes($"Linked to Debt Payoff Plan ID: {payoffPlanId}");

            await _goalRepository.InsertAsync(goal, autoSave: true);

            _logger.LogInformation(
                "Financial goal created from payoff plan: {GoalId} - {GoalName}",
                goal.Id,
                goal.Name);

            return goal;
        }

        /// <summary>
        /// Update financial goal progress based on debt payment
        /// </summary>
        [UnitOfWork]
        public virtual async Task UpdateGoalProgressFromDebtPaymentAsync(
            Guid payoffPlanId,
            decimal principalPaid,
            decimal newRemainingBalance,
            Guid? tenantId = null)
        {
            // Find goal linked to this payoff plan
            var goals = await _goalRepository.GetListAsync();
            var linkedGoal = goals.Find(g =>
                g.TenantId == tenantId &&
                g.Type == GoalType.DebtPayoff &&
                g.Notes != null &&
                g.Notes.Contains(payoffPlanId.ToString()));

            if (linkedGoal == null)
            {
                _logger.LogWarning(
                    "No linked financial goal found for payoff plan {PayoffPlanId}",
                    payoffPlanId);
                return;
            }

            // Update progress: Current Amount = Total Debt - Remaining Balance
            var totalDebt = linkedGoal.TargetAmount.Amount;
            var paidSoFar = totalDebt - newRemainingBalance;
            var progressAmount = new MonetaryValue(paidSoFar, linkedGoal.TargetAmount.Currency);

            linkedGoal.UpdateProgress(progressAmount);

            await _goalRepository.UpdateAsync(linkedGoal, autoSave: true);

            _logger.LogInformation(
                "Updated goal progress: {GoalName} - {Progress}%",
                linkedGoal.Name,
                linkedGoal.ProgressPercentage);
        }
    }
}
