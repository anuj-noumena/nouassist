using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Entities;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Services.Interfaces
{
    /// <summary>
    /// Domain service for revenue simulation and projection
    /// </summary>
    public interface IRevenueSimulationEngine
    {
        Task<List<ProjectedRevenueData>> SimulateRevenueAsync(
            RevenueScenario scenario,
            DateTime simulationDate);

        Task<ProjectedRevenueData> ProjectRevenueForMonthAsync(
            ScenarioParameters parameters,
            int monthOffset,
            DateTime baseDate);

        Task<List<ProjectedRevenueData>> CompareScenarios(
            List<RevenueScenario> scenarios,
            DateTime simulationDate,
            int months);

        Task<Dictionary<string, decimal>> CalculateKeyMetricsAsync(
            RevenueScenario scenario);

        Task<ValidationResult> ValidateScenarioParametersAsync(ScenarioParameters parameters);
    }
}
