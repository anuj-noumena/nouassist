using System;
using System.Linq;
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
    /// Domain service for integrating investment portfolios with financial goals
    /// </summary>
    public class InvestmentGoalIntegrationService : DomainService
    {
        private readonly IFinancialGoalRepository _goalRepository;
        private readonly ILogger<InvestmentGoalIntegrationService> _logger;

        public InvestmentGoalIntegrationService(
            IFinancialGoalRepository goalRepository,
            ILogger<InvestmentGoalIntegrationService> logger)
        {
            _goalRepository = goalRepository;
            _logger = logger;
        }

        /// <summary>
        /// Update net worth goal based on portfolio value change
        /// </summary>
        [UnitOfWork]
        public virtual async Task UpdateNetWorthGoalFromPortfolioAsync(
            Guid portfolioId,
            string portfolioName,
            decimal portfolioCurrentValue,
            string currency,
            Guid userId,
            Guid? tenantId = null)
        {
            _logger.LogInformation(
                "Updating net worth goal from portfolio: {PortfolioName} - {CurrentValue}",
                portfolioName,
                portfolioCurrentValue);

            // Find or create net worth goal
            var goals = await _goalRepository.GetListAsync();
            var netWorthGoal = goals.FirstOrDefault(g =>
                g.TenantId == tenantId &&
                g.UserId == userId &&
                g.Type == GoalType.NetWorth);

            if (netWorthGoal == null)
            {
                _logger.LogInformation("Creating new net worth goal for user {UserId}", userId);

                netWorthGoal = new FinancialGoal(
                    id: GuidGenerator.Create(),
                    tenantId: tenantId,
                    userId: userId,
                    name: "Build Net Worth",
                    type: GoalType.NetWorth,
                    targetAmount: new MonetaryValue(1000000, currency), // Default 1M target
                    currentAmount: new MonetaryValue(portfolioCurrentValue, currency),
                    targetDate: DateTime.UtcNow.AddYears(10), // Default 10 year goal
                    description: "Total net worth across all assets"
                );

                await _goalRepository.InsertAsync(netWorthGoal, autoSave: true);
            }
            else
            {
                // Update current amount (would aggregate all portfolios in production)
                var newAmount = new MonetaryValue(portfolioCurrentValue, currency);
                netWorthGoal.UpdateProgress(newAmount);

                await _goalRepository.UpdateAsync(netWorthGoal, autoSave: true);
            }

            _logger.LogInformation(
                "Net worth goal updated: Current {Current} / Target {Target} ({Progress}%)",
                netWorthGoal.CurrentAmount.Amount,
                netWorthGoal.TargetAmount.Amount,
                netWorthGoal.ProgressPercentage);
        }

        /// <summary>
        /// Update retirement goal based on portfolio value (if portfolio is linked to retirement)
        /// </summary>
        [UnitOfWork]
        public virtual async Task UpdateRetirementGoalFromPortfolioAsync(
            Guid portfolioId,
            string portfolioName,
            decimal portfolioCurrentValue,
            string currency,
            Guid userId,
            Guid? tenantId = null)
        {
            // Find retirement goal linked to this portfolio
            var goals = await _goalRepository.GetListAsync();
            var retirementGoal = goals.FirstOrDefault(g =>
                g.TenantId == tenantId &&
                g.UserId == userId &&
                g.Type == GoalType.Retirement &&
                g.Notes != null &&
                g.Notes.Contains(portfolioId.ToString()));

            if (retirementGoal == null)
            {
                _logger.LogDebug(
                    "No retirement goal linked to portfolio {PortfolioId}",
                    portfolioId);
                return;
            }

            // Update progress
            var newAmount = new MonetaryValue(portfolioCurrentValue, currency);
            retirementGoal.UpdateProgress(newAmount);

            await _goalRepository.UpdateAsync(retirementGoal, autoSave: true);

            _logger.LogInformation(
                "Retirement goal updated from portfolio {PortfolioName}: {Progress}%",
                portfolioName,
                retirementGoal.ProgressPercentage);
        }

        /// <summary>
        /// Track investment contribution in budget (e.g., 401k contribution, IRA deposit)
        /// </summary>
        public decimal CalculateMonthlyInvestmentContribution(
            decimal targetAmount,
            decimal currentAmount,
            int monthsRemaining,
            decimal expectedReturn)
        {
            if (monthsRemaining <= 0)
            {
                return 0;
            }

            // Simplified calculation - in production use proper time-value-of-money formula
            var remainingAmount = targetAmount - currentAmount;
            var monthlyContribution = remainingAmount / monthsRemaining;

            // Adjust for expected returns (very simplified)
            if (expectedReturn > 0)
            {
                monthlyContribution = monthlyContribution / (1 + (expectedReturn / 12));
            }

            return Math.Max(0, monthlyContribution);
        }
    }
}
