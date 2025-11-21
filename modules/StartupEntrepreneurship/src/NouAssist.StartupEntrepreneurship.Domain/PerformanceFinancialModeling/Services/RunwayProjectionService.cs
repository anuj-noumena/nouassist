using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Aggregates;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Services
{
    /// <summary>
    /// Domain service for runway calculations and projections
    /// </summary>
    public class RunwayProjectionService : DomainService, IRunwayProjectionService
    {
        private readonly IFinancialModelingEngine _modelingEngine;
        private readonly ILogger<RunwayProjectionService> _logger;

        public RunwayProjectionService(
            IFinancialModelingEngine modelingEngine,
            ILogger<RunwayProjectionService> logger)
        {
            _modelingEngine = modelingEngine;
            _logger = logger;
        }

        public async Task<RunwayLength> CalculateCurrentRunwayAsync(FinancialModel model, ModelScenario scenario)
        {
            try
            {
                var cashFlowProjection = await _modelingEngine.CalculateCashFlowProjectionAsync(model, scenario);
                var runwayMonths = cashFlowProjection.GetRunwayMonths() ?? 0;

                var burnRate = await CalculateBurnRateAsync(model, scenario);
                var assumptions = GetScenarioAssumptions(model, scenario);
                var currentCash = assumptions.GetValueOrDefault("InitialCash", 100000m);

                var depletionDate = DateTime.UtcNow.AddMonths(runwayMonths);

                return new RunwayLength(runwayMonths, depletionDate, burnRate.MonthlyAmount, currentCash);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate runway for model {ModelId}, scenario {ScenarioId}", 
                               model.Id, scenario.Id);
                throw;
            }
        }

        public async Task<BurnRateValue> CalculateBurnRateAsync(FinancialModel model, ModelScenario scenario, int periodMonths = 3)
        {
            try
            {
                var assumptions = GetScenarioAssumptions(model, scenario);
                var monthlyRevenue = assumptions.GetValueOrDefault("MonthlyRevenue", 0m);
                var monthlyExpenses = assumptions.GetValueOrDefault("MonthlyExpenses", 0m);
                
                var monthlyBurnRate = monthlyExpenses - monthlyRevenue;
                
                // Ensure burn rate is positive (cash outflow)
                monthlyBurnRate = Math.Max(0, monthlyBurnRate);

                return new BurnRateValue(monthlyBurnRate, DateTime.UtcNow, periodMonths);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate burn rate for model {ModelId}, scenario {ScenarioId}", 
                               model.Id, scenario.Id);
                throw;
            }
        }

        public async Task<List<RunwayProjection>> ProjectRunwayAsync(FinancialModel model, ModelScenario scenario, int monthsAhead = 12)
        {
            try
            {
                var projections = new List<RunwayProjection>();
                var cashFlowProjection = await _modelingEngine.CalculateCashFlowProjectionAsync(model, scenario);
                var burnRate = await CalculateBurnRateAsync(model, scenario);

                for (int month = 1; month <= monthsAhead; month++)
                {
                    var projectedAmount = cashFlowProjection.GetProjectionForPeriod(month);
                    if (projectedAmount != null)
                    {
                        var remainingMonths = projectedAmount.Amount > 0 
                            ? (int)(projectedAmount.Amount / burnRate.MonthlyAmount)
                            : 0;

                        var status = GetRunwayStatus(remainingMonths);

                        projections.Add(new RunwayProjection
                        {
                            MonthIndex = month,
                            Date = projectedAmount.ProjectionDate,
                            CashPosition = projectedAmount.Amount,
                            BurnRate = burnRate.MonthlyAmount,
                            RemainingMonths = remainingMonths,
                            Status = status
                        });
                    }
                }

                return projections;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to project runway for model {ModelId}, scenario {ScenarioId}", 
                               model.Id, scenario.Id);
                throw;
            }
        }

        public async Task<RunwayOptimization> OptimizeRunwayAsync(FinancialModel model, ModelScenario scenario, int targetMonths = 18)
        {
            try
            {
                var currentRunway = await CalculateCurrentRunwayAsync(model, scenario);
                var assumptions = GetScenarioAssumptions(model, scenario);

                var options = new List<OptimizationOption>();

                // Revenue optimization options
                options.Add(new OptimizationOption
                {
                    Type = "Revenue Increase",
                    Description = "Increase monthly revenue by 20%",
                    Impact = assumptions.GetValueOrDefault("MonthlyRevenue", 0m) * 0.2m,
                    CostSavings = 0,
                    AdditionalMonths = CalculateAdditionalMonths(currentRunway, assumptions.GetValueOrDefault("MonthlyRevenue", 0m) * 0.2m, true),
                    Difficulty = "Medium"
                });

                // Cost reduction options
                var currentExpenses = assumptions.GetValueOrDefault("MonthlyExpenses", 0m);
                options.Add(new OptimizationOption
                {
                    Type = "Cost Reduction",
                    Description = "Reduce monthly expenses by 15%",
                    Impact = currentExpenses * 0.15m,
                    CostSavings = currentExpenses * 0.15m,
                    AdditionalMonths = CalculateAdditionalMonths(currentRunway, currentExpenses * 0.15m, false),
                    Difficulty = "Easy"
                });

                // Funding options
                var fundingRequired = currentRunway.GetEstimatedFundingNeeded(targetMonths);
                options.Add(new OptimizationOption
                {
                    Type = "Additional Funding",
                    Description = $"Raise ${fundingRequired:N0} in funding",
                    Impact = fundingRequired,
                    CostSavings = 0,
                    AdditionalMonths = targetMonths - currentRunway.Months,
                    Difficulty = "Hard"
                });

                // Calculate optimized runway with best options
                var optimizedRunway = currentRunway.ExtendWith(fundingRequired);

                var recommendations = GenerateRecommendations(currentRunway, targetMonths, options);

                return new RunwayOptimization
                {
                    CurrentRunway = currentRunway,
                    OptimizedRunway = optimizedRunway,
                    Options = options,
                    FundingRequired = fundingRequired,
                    Recommendations = recommendations
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to optimize runway for model {ModelId}, scenario {ScenarioId}", 
                               model.Id, scenario.Id);
                throw;
            }
        }

        public async Task<List<RunwayAlert>> CheckRunwayAlertsAsync(FinancialModel model, List<ModelScenario> scenarios)
        {
            var alerts = new List<RunwayAlert>();

            foreach (var scenario in scenarios)
            {
                try
                {
                    var runway = await CalculateCurrentRunwayAsync(model, scenario);
                    var runwayAlerts = GenerateRunwayAlerts(model, scenario, runway);
                    alerts.AddRange(runwayAlerts);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to check runway alerts for scenario {ScenarioId}", scenario.Id);
                }
            }

            return alerts.OrderByDescending(a => a.Priority).ToList();
        }

        private Dictionary<string, decimal> GetScenarioAssumptions(FinancialModel model, ModelScenario scenario)
        {
            var allAssumptions = new Dictionary<string, decimal>();

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

            return allAssumptions;
        }

        private string GetRunwayStatus(int remainingMonths)
        {
            return remainingMonths switch
            {
                <= 3 => "Critical",
                <= 6 => "Low",
                <= 12 => "Medium",
                _ => "Healthy"
            };
        }

        private int CalculateAdditionalMonths(RunwayLength currentRunway, decimal impact, bool isRevenueIncrease)
        {
            var currentBurnRate = currentRunway.CurrentBurnRate;
            var newBurnRate = isRevenueIncrease 
                ? Math.Max(0, currentBurnRate - impact)  // Revenue reduces burn rate
                : Math.Max(0, currentBurnRate - impact); // Cost reduction reduces burn rate

            if (newBurnRate <= 0) return int.MaxValue; // Positive cash flow

            var newRunwayMonths = (int)(currentRunway.CurrentCashBalance / newBurnRate);
            return newRunwayMonths - currentRunway.Months;
        }

        private List<string> GenerateRecommendations(RunwayLength currentRunway, int targetMonths, List<OptimizationOption> options)
        {
            var recommendations = new List<string>();

            if (currentRunway.IsCritical())
            {
                recommendations.Add("URGENT: Runway is critically low. Immediate action required.");
                recommendations.Add("Consider emergency cost cutting measures.");
                recommendations.Add("Accelerate fundraising efforts.");
            }
            else if (currentRunway.IsRunningOut())
            {
                recommendations.Add("Warning: Runway is running low. Start planning for extension.");
                recommendations.Add("Review and optimize operational expenses.");
                recommendations.Add("Explore revenue acceleration opportunities.");
            }

            if (currentRunway.Months < targetMonths)
            {
                var shortfall = targetMonths - currentRunway.Months;
                recommendations.Add($"Need to extend runway by {shortfall} months to reach target.");
                
                var bestOption = options.OrderByDescending(o => o.AdditionalMonths / Math.Max(1, GetDifficultyScore(o.Difficulty))).First();
                recommendations.Add($"Best option: {bestOption.Description} (adds {bestOption.AdditionalMonths} months)");
            }
            else
            {
                recommendations.Add("Runway is currently healthy.");
                recommendations.Add("Continue monitoring burn rate and cash position.");
            }

            return recommendations;
        }

        private List<RunwayAlert> GenerateRunwayAlerts(FinancialModel model, ModelScenario scenario, RunwayLength runway)
        {
            var alerts = new List<RunwayAlert>();

            if (runway.IsCritical(3))
            {
                alerts.Add(new RunwayAlert
                {
                    ModelId = model.Id,
                    ModelName = model.Name,
                    ScenarioName = scenario.Name,
                    RemainingMonths = runway.Months,
                    AlertType = "Critical",
                    Message = $"CRITICAL: Only {runway.Months} months of runway remaining",
                    Priority = 3,
                    AlertDate = DateTime.UtcNow
                });
            }
            else if (runway.IsRunningOut(6))
            {
                alerts.Add(new RunwayAlert
                {
                    ModelId = model.Id,
                    ModelName = model.Name,
                    ScenarioName = scenario.Name,
                    RemainingMonths = runway.Months,
                    AlertType = "Warning",
                    Message = $"Warning: {runway.Months} months of runway remaining",
                    Priority = 2,
                    AlertDate = DateTime.UtcNow
                });
            }
            else if (runway.IsRunningOut(12))
            {
                alerts.Add(new RunwayAlert
                {
                    ModelId = model.Id,
                    ModelName = model.Name,
                    ScenarioName = scenario.Name,
                    RemainingMonths = runway.Months,
                    AlertType = "Info",
                    Message = $"Info: {runway.Months} months of runway - start planning for extension",
                    Priority = 1,
                    AlertDate = DateTime.UtcNow
                });
            }

            return alerts;
        }

        private int GetDifficultyScore(string difficulty)
        {
            return difficulty switch
            {
                "Easy" => 1,
                "Medium" => 2,
                "Hard" => 3,
                _ => 2
            };
        }
    }
}
