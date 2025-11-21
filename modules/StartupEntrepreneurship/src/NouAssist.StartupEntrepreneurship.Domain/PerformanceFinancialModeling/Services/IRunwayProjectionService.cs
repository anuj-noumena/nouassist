using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Aggregates;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Services
{
    /// <summary>
    /// Service for calculating and projecting runway
    /// </summary>
    public interface IRunwayProjectionService
    {
        Task<RunwayLength> CalculateCurrentRunwayAsync(FinancialModel model, ModelScenario scenario);
        Task<BurnRateValue> CalculateBurnRateAsync(FinancialModel model, ModelScenario scenario, int periodMonths = 3);
        Task<List<RunwayProjection>> ProjectRunwayAsync(FinancialModel model, ModelScenario scenario, int monthsAhead = 12);
        Task<RunwayOptimization> OptimizeRunwayAsync(FinancialModel model, ModelScenario scenario, int targetMonths = 18);
        Task<List<RunwayAlert>> CheckRunwayAlertsAsync(FinancialModel model, List<ModelScenario> scenarios);
    }

    public class RunwayProjection
    {
        public int MonthIndex { get; set; }
        public DateTime Date { get; set; }
        public decimal CashPosition { get; set; }
        public decimal BurnRate { get; set; }
        public int RemainingMonths { get; set; }
        public string Status { get; set; }
    }

    public class RunwayOptimization
    {
        public RunwayLength CurrentRunway { get; set; }
        public RunwayLength OptimizedRunway { get; set; }
        public List<OptimizationOption> Options { get; set; }
        public decimal FundingRequired { get; set; }
        public List<string> Recommendations { get; set; }
    }

    public class OptimizationOption
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public decimal Impact { get; set; }
        public decimal CostSavings { get; set; }
        public int AdditionalMonths { get; set; }
        public string Difficulty { get; set; }
    }

    public class RunwayAlert
    {
        public Guid ModelId { get; set; }
        public string ModelName { get; set; }
        public string ScenarioName { get; set; }
        public int RemainingMonths { get; set; }
        public string AlertType { get; set; }
        public string Message { get; set; }
        public int Priority { get; set; }
        public DateTime AlertDate { get; set; }
    }
}
