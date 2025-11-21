using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Entities;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Services.Interfaces;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Services
{
    /// <summary>
    /// Domain service for revenue simulation and projection
    /// </summary>
    public class RevenueSimulationEngine : DomainService, IRevenueSimulationEngine
    {
        private readonly ILogger<RevenueSimulationEngine> _logger;

        public RevenueSimulationEngine(ILogger<RevenueSimulationEngine> logger)
        {
            _logger = logger;
        }

        public async Task<List<ProjectedRevenueData>> SimulateRevenueAsync(
            RevenueScenario scenario,
            DateTime simulationDate)
        {
            try
            {
                var projections = new List<ProjectedRevenueData>();
                var parameters = scenario.Parameters;

                for (int month = 0; month < parameters.TimeHorizonMonths; month++)
                {
                    var projection = await ProjectRevenueForMonthAsync(parameters, month, simulationDate);
                    projections.Add(projection);
                }

                // Update scenario with projections
                scenario.ClearProjections();
                foreach (var projection in projections)
                {
                    scenario.AddProjection(projection);
                }

                scenario.MarkAsSimulated(simulationDate);

                _logger.LogInformation("Revenue simulation completed for scenario {ScenarioId}, generated {ProjectionCount} projections",
                    scenario.Id, projections.Count);

                return projections;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to simulate revenue for scenario {ScenarioId}", scenario.Id);
                throw;
            }
        }

        public async Task<ProjectedRevenueData> ProjectRevenueForMonthAsync(
            ScenarioParameters parameters,
            int monthOffset,
            DateTime baseDate)
        {
            var projectionDate = baseDate.AddMonths(monthOffset);
            
            // Apply growth and churn
            var customersAtMonth = CalculateCustomersAtMonth(parameters, monthOffset);
            var monthlyRecurringRevenue = customersAtMonth * parameters.AverageRevenuePerUser;
            var annualRecurringRevenue = monthlyRecurringRevenue * 12;
            
            // Calculate costs
            var totalCosts = customersAtMonth * parameters.CostPerCustomer;
            var netRevenue = monthlyRecurringRevenue - totalCosts;
            
            // Calculate metrics
            var customerAcquisitionCost = CalculateCustomerAcquisitionCost(parameters, monthOffset);
            var lifetimeValue = CalculateLifetimeValue(parameters);

            return new ProjectedRevenueData(
                monthlyRecurringRevenue,
                annualRecurringRevenue,
                monthlyRecurringRevenue, // Total revenue for simplicity
                netRevenue,
                customersAtMonth,
                customerAcquisitionCost,
                lifetimeValue,
                parameters.ChurnRate,
                projectionDate
            );
        }

        public async Task<List<ProjectedRevenueData>> CompareScenarios(
            List<RevenueScenario> scenarios,
            DateTime simulationDate,
            int months)
        {
            var comparisonResults = new List<ProjectedRevenueData>();

            foreach (var scenario in scenarios)
            {
                var projections = await SimulateRevenueAsync(scenario, simulationDate);
                var finalProjection = projections.LastOrDefault();
                if (finalProjection != null)
                {
                    comparisonResults.Add(finalProjection);
                }
            }

            return comparisonResults;
        }

        public async Task<Dictionary<string, decimal>> CalculateKeyMetricsAsync(RevenueScenario scenario)
        {
            var metrics = new Dictionary<string, decimal>();
            var latestProjection = scenario.Projections.OrderByDescending(p => p.ProjectionDate).FirstOrDefault();

            if (latestProjection == null)
            {
                return metrics;
            }

            metrics["MRR"] = latestProjection.MonthlyRecurringRevenue;
            metrics["ARR"] = latestProjection.AnnualRecurringRevenue;
            metrics["NetRevenue"] = latestProjection.NetRevenue;
            metrics["CustomerCount"] = latestProjection.CustomerCount;
            metrics["CAC"] = latestProjection.CustomerAcquisitionCost;
            metrics["LTV"] = latestProjection.LifetimeValue;
            metrics["LTV_CAC_Ratio"] = latestProjection.CustomerAcquisitionCost > 0 
                ? latestProjection.LifetimeValue / latestProjection.CustomerAcquisitionCost 
                : 0;
            metrics["ChurnRate"] = (decimal)latestProjection.ChurnRate;

            return metrics;
        }

        public async Task<ValidationResult> ValidateScenarioParametersAsync(ScenarioParameters parameters)
        {
            var result = new ValidationResult();

            if (parameters.CustomerCount < 0)
                result.AddError("Customer count cannot be negative");

            if (parameters.AverageRevenuePerUser < 0)
                result.AddError("Average revenue per user cannot be negative");

            if (parameters.ChurnRate < 0 || parameters.ChurnRate > 1)
                result.AddError("Churn rate must be between 0 and 1");

            if (parameters.GrowthRate < -1)
                result.AddError("Growth rate cannot be less than -100%");

            if (parameters.TimeHorizonMonths <= 0)
                result.AddError("Time horizon must be positive");

            if (parameters.CostPerCustomer < 0)
                result.AddError("Cost per customer cannot be negative");

            // Business logic validations
            if (parameters.ChurnRate > 0.5)
                result.AddWarning("Churn rate above 50% is unusually high");

            if (parameters.GrowthRate > 2.0)
                result.AddWarning("Growth rate above 200% may be unrealistic");

            return result;
        }

        private int CalculateCustomersAtMonth(ScenarioParameters parameters, int month)
        {
            var customers = (double)parameters.CustomerCount;
            
            for (int i = 0; i < month; i++)
            {
                // Apply churn
                customers = customers * (1 - parameters.ChurnRate);
                
                // Apply growth
                customers = customers * (1 + parameters.GrowthRate);
            }

            return (int)Math.Round(customers);
        }

        private decimal CalculateCustomerAcquisitionCost(ScenarioParameters parameters, int month)
        {
            // Simple CAC calculation - in reality this would be more complex
            var baseCac = parameters.AverageRevenuePerUser * 0.3m; // 30% of ARPU as baseline
            
            // CAC might increase over time as market becomes more competitive
            var cagGrowthFactor = 1 + (month * 0.02m); // 2% increase per month
            
            return baseCac * cagGrowthFactor;
        }

        private decimal CalculateLifetimeValue(ScenarioParameters parameters)
        {
            if (parameters.ChurnRate <= 0)
                return decimal.MaxValue;

            // LTV = ARPU / Churn Rate
            return parameters.AverageRevenuePerUser / (decimal)parameters.ChurnRate;
        }
    }
}
