using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Aggregates;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Services
{
    /// <summary>
    /// Domain service for financial modeling calculations
    /// </summary>
    public class FinancialModelingEngine : DomainService, IFinancialModelingEngine
    {
        private readonly ILogger<FinancialModelingEngine> _logger;

        public FinancialModelingEngine(ILogger<FinancialModelingEngine> logger)
        {
            _logger = logger;
        }

        public async Task<ProjectionResult> CalculateCashFlowProjectionAsync(FinancialModel model, ModelScenario scenario)
        {
            await Task.CompletedTask;

            try
            {
                var assumptions = GetScenarioAssumptions(model, scenario);
                var projectedAmounts = new List<ProjectedAmount>();

                // Sample cash flow calculation logic
                var initialCash = assumptions.GetValueOrDefault("InitialCash", 100000m);
                var monthlyRevenue = assumptions.GetValueOrDefault("MonthlyRevenue", 10000m);
                var monthlyExpenses = assumptions.GetValueOrDefault("MonthlyExpenses", 8000m);
                var revenueGrowthRate = assumptions.GetValueOrDefault("RevenueGrowthRate", 0.05m);
                var expenseGrowthRate = assumptions.GetValueOrDefault("ExpenseGrowthRate", 0.02m);

                var currentCash = initialCash;
                var currentRevenue = monthlyRevenue;
                var currentExpenses = monthlyExpenses;

                for (int period = 1; period <= model.ProjectionYears * 12; period++)
                {
                    var netCashFlow = currentRevenue - currentExpenses;
                    currentCash += netCashFlow;

                    var projectionDate = model.BaseDate.AddMonths(period);
                    var projectedAmount = new ProjectedAmount(currentCash, period, projectionDate);
                    projectedAmounts.Add(projectedAmount);

                    // Apply growth rates
                    currentRevenue *= (1 + revenueGrowthRate / 12);
                    currentExpenses *= (1 + expenseGrowthRate / 12);
                }

                return new ProjectionResult(model.Id, scenario.Id, ProjectionType.CashFlow, projectedAmounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate cash flow projection for model {ModelId}, scenario {ScenarioId}", 
                               model.Id, scenario.Id);
                throw;
            }
        }

        public async Task<ProjectionResult> CalculateProfitAndLossProjectionAsync(FinancialModel model, ModelScenario scenario)
        {
            await Task.CompletedTask;

            try
            {
                var assumptions = GetScenarioAssumptions(model, scenario);
                var projectedAmounts = new List<ProjectedAmount>();

                var monthlyRevenue = assumptions.GetValueOrDefault("MonthlyRevenue", 10000m);
                var directCosts = assumptions.GetValueOrDefault("DirectCostsPercentage", 0.3m);
                var operatingExpenses = assumptions.GetValueOrDefault("MonthlyOperatingExpenses", 5000m);
                var revenueGrowthRate = assumptions.GetValueOrDefault("RevenueGrowthRate", 0.05m);

                var currentRevenue = monthlyRevenue;
                var currentOpex = operatingExpenses;

                for (int period = 1; period <= model.ProjectionYears * 12; period++)
                {
                    var grossProfit = currentRevenue * (1 - directCosts);
                    var netProfit = grossProfit - currentOpex;

                    var projectionDate = model.BaseDate.AddMonths(period);
                    var projectedAmount = new ProjectedAmount(netProfit, period, projectionDate);
                    projectedAmounts.Add(projectedAmount);

                    currentRevenue *= (1 + revenueGrowthRate / 12);
                    currentOpex *= (1 + assumptions.GetValueOrDefault("OpexGrowthRate", 0.03m) / 12);
                }

                return new ProjectionResult(model.Id, scenario.Id, ProjectionType.ProfitAndLoss, projectedAmounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate P&L projection for model {ModelId}, scenario {ScenarioId}", 
                               model.Id, scenario.Id);
                throw;
            }
        }

        public async Task<ProjectionResult> CalculateRevenueProjectionAsync(FinancialModel model, ModelScenario scenario)
        {
            await Task.CompletedTask;

            try
            {
                var assumptions = GetScenarioAssumptions(model, scenario);
                var projectedAmounts = new List<ProjectedAmount>();

                var monthlyRevenue = assumptions.GetValueOrDefault("MonthlyRevenue", 10000m);
                var growthRate = assumptions.GetValueOrDefault("RevenueGrowthRate", 0.05m);
                var seasonalityFactor = assumptions.GetValueOrDefault("SeasonalityFactor", 0.1m);

                var currentRevenue = monthlyRevenue;

                for (int period = 1; period <= model.ProjectionYears * 12; period++)
                {
                    // Apply seasonality (simplified sine wave)
                    var seasonalAdjustment = 1 + seasonalityFactor * Math.Sin(2 * Math.PI * period / 12);
                    var adjustedRevenue = currentRevenue * (decimal)seasonalAdjustment;

                    var projectionDate = model.BaseDate.AddMonths(period);
                    var projectedAmount = new ProjectedAmount(adjustedRevenue, period, projectionDate);
                    projectedAmounts.Add(projectedAmount);

                    currentRevenue *= (1 + growthRate / 12);
                }

                return new ProjectionResult(model.Id, scenario.Id, ProjectionType.Revenue, projectedAmounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate revenue projection for model {ModelId}, scenario {ScenarioId}", 
                               model.Id, scenario.Id);
                throw;
            }
        }

        public async Task<ProjectionResult> CalculateExpenseProjectionAsync(FinancialModel model, ModelScenario scenario)
        {
            await Task.CompletedTask;

            try
            {
                var assumptions = GetScenarioAssumptions(model, scenario);
                var projectedAmounts = new List<ProjectedAmount>();

                var fixedExpenses = assumptions.GetValueOrDefault("MonthlyFixedExpenses", 5000m);
                var variableExpenseRate = assumptions.GetValueOrDefault("VariableExpenseRate", 0.2m);
                var monthlyRevenue = assumptions.GetValueOrDefault("MonthlyRevenue", 10000m);
                var expenseGrowthRate = assumptions.GetValueOrDefault("ExpenseGrowthRate", 0.03m);
                var revenueGrowthRate = assumptions.GetValueOrDefault("RevenueGrowthRate", 0.05m);

                var currentFixedExpenses = fixedExpenses;
                var currentRevenue = monthlyRevenue;

                for (int period = 1; period <= model.ProjectionYears * 12; period++)
                {
                    var variableExpenses = currentRevenue * variableExpenseRate;
                    var totalExpenses = currentFixedExpenses + variableExpenses;

                    var projectionDate = model.BaseDate.AddMonths(period);
                    var projectedAmount = new ProjectedAmount(totalExpenses, period, projectionDate);
                    projectedAmounts.Add(projectedAmount);

                    currentFixedExpenses *= (1 + expenseGrowthRate / 12);
                    currentRevenue *= (1 + revenueGrowthRate / 12);
                }

                return new ProjectionResult(model.Id, scenario.Id, ProjectionType.Expense, projectedAmounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate expense projection for model {ModelId}, scenario {ScenarioId}", 
                               model.Id, scenario.Id);
                throw;
            }
        }

        public async Task<ProjectionResult> CalculateBreakEvenAnalysisAsync(FinancialModel model, ModelScenario scenario)
        {
            await Task.CompletedTask;

            try
            {
                var revenueProjection = await CalculateRevenueProjectionAsync(model, scenario);
                var expenseProjection = await CalculateExpenseProjectionAsync(model, scenario);

                var projectedAmounts = new List<ProjectedAmount>();

                for (int period = 1; period <= model.ProjectionYears * 12; period++)
                {
                    var revenue = revenueProjection.GetProjectionForPeriod(period)?.Amount ?? 0;
                    var expenses = expenseProjection.GetProjectionForPeriod(period)?.Amount ?? 0;
                    var netProfit = revenue - expenses;

                    var projectionDate = model.BaseDate.AddMonths(period);
                    var projectedAmount = new ProjectedAmount(netProfit, period, projectionDate);
                    projectedAmounts.Add(projectedAmount);
                }

                return new ProjectionResult(model.Id, scenario.Id, ProjectionType.BreakEven, projectedAmounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate break-even analysis for model {ModelId}, scenario {ScenarioId}", 
                               model.Id, scenario.Id);
                throw;
            }
        }

        public async Task<int?> CalculateRunwayProjectionAsync(FinancialModel model, ModelScenario scenario)
        {
            var cashFlowProjection = await CalculateCashFlowProjectionAsync(model, scenario);
            return cashFlowProjection.GetRunwayMonths();
        }

        public async Task<List<ProjectionResult>> CalculateAllProjectionsAsync(FinancialModel model, ModelScenario scenario)
        {
            var projections = new List<ProjectionResult>();

            try
            {
                projections.Add(await CalculateCashFlowProjectionAsync(model, scenario));
                projections.Add(await CalculateProfitAndLossProjectionAsync(model, scenario));
                projections.Add(await CalculateRevenueProjectionAsync(model, scenario));
                projections.Add(await CalculateExpenseProjectionAsync(model, scenario));
                projections.Add(await CalculateBreakEvenAnalysisAsync(model, scenario));

                return projections;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate all projections for model {ModelId}, scenario {ScenarioId}", 
                               model.Id, scenario.Id);
                throw;
            }
        }

        private Dictionary<string, decimal> GetScenarioAssumptions(FinancialModel model, ModelScenario scenario)
        {
            var allAssumptions = new Dictionary<string, decimal>();

            // Combine assumptions from all associated assumption sets
            foreach (var assumptionSetId in scenario.AssociatedAssumptionSets)
            {
                var assumptionSet = model.GetAssumptionSet(assumptionSetId);
                if (assumptionSet != null)
                {
                    foreach (var assumption in assumptionSet.Assumptions)
                    {
                        allAssumptions[assumption.Name] = assumption.Value;
                    }
                }
            }

            // Apply scenario-specific multipliers based on scenario type
            switch (scenario.Type)
            {
                case ScenarioType.Optimistic:
                    ApplyScenarioMultipliers(allAssumptions, 1.2m, 0.9m); // 20% higher revenue, 10% lower costs
                    break;
                case ScenarioType.Pessimistic:
                    ApplyScenarioMultipliers(allAssumptions, 0.8m, 1.1m); // 20% lower revenue, 10% higher costs
                    break;
                case ScenarioType.Conservative:
                    ApplyScenarioMultipliers(allAssumptions, 0.9m, 1.05m); // 10% lower revenue, 5% higher costs
                    break;
                case ScenarioType.Aggressive:
                    ApplyScenarioMultipliers(allAssumptions, 1.3m, 1.0m); // 30% higher revenue, same costs
                    break;
            }

            return allAssumptions;
        }

        private void ApplyScenarioMultipliers(Dictionary<string, decimal> assumptions, decimal revenueMultiplier, decimal costMultiplier)
        {
            var revenueKeys = assumptions.Keys.Where(k => k.Contains("Revenue", StringComparison.OrdinalIgnoreCase)).ToList();
            var costKeys = assumptions.Keys.Where(k => k.Contains("Expense", StringComparison.OrdinalIgnoreCase) || 
                                                      k.Contains("Cost", StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var key in revenueKeys)
            {
                assumptions[key] *= revenueMultiplier;
            }

            foreach (var key in costKeys)
            {
                assumptions[key] *= costMultiplier;
            }
        }
    }
}
