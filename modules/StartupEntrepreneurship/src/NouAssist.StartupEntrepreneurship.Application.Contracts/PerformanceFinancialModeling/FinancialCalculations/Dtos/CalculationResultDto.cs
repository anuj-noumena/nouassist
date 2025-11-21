using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Dtos.Base;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.FinancialCalculations.Dtos
{
    /// <summary>
    /// Base DTO for calculation results
    /// </summary>
    public class CalculationResultDto
    {
        public Guid ModelId { get; set; }
        public string ModelName { get; set; }
        public Guid ScenarioId { get; set; }
        public string ScenarioName { get; set; }
        public DateTime CalculatedAt { get; set; }
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public string Notes { get; set; }
    }

    /// <summary>
    /// DTO for sensitivity analysis results
    /// </summary>
    public class SensitivityAnalysisResultDto : CalculationResultDto
    {
        public string VariableName { get; set; }
        public decimal BaseValue { get; set; }
        public decimal BaseResult { get; set; }
        public Dictionary<decimal, decimal> SensitivityPoints { get; set; } = new Dictionary<decimal, decimal>();
        public decimal Elasticity { get; set; }
        public decimal MaxImpact { get; set; }
        public decimal MinImpact { get; set; }
        public bool IsHighSensitivity { get; set; }
        public List<decimal> RiskRanges { get; set; } = new List<decimal>();
    }

    /// <summary>
    /// DTO for runway analysis results
    /// </summary>
    public class RunwayAnalysisResultDto : CalculationResultDto
    {
        public RunwayLengthDto CurrentRunway { get; set; }
        public BurnRateValueDto BurnRate { get; set; }
        public List<RunwayProjectionDto> Projections { get; set; } = new List<RunwayProjectionDto>();
        public List<RunwayAlertDto> Alerts { get; set; } = new List<RunwayAlertDto>();
        public RunwayOptimizationDto Optimization { get; set; }
    }

    /// <summary>
    /// DTO for runway projections
    /// </summary>
    public class RunwayProjectionDto
    {
        public int MonthIndex { get; set; }
        public DateTime Date { get; set; }
        public decimal CashPosition { get; set; }
        public decimal BurnRate { get; set; }
        public int RemainingMonths { get; set; }
        public string Status { get; set; }
    }

    /// <summary>
    /// DTO for runway alerts
    /// </summary>
    public class RunwayAlertDto
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

    /// <summary>
    /// DTO for runway optimization
    /// </summary>
    public class RunwayOptimizationDto
    {
        public RunwayLengthDto CurrentRunway { get; set; }
        public RunwayLengthDto OptimizedRunway { get; set; }
        public List<OptimizationOptionDto> Options { get; set; } = new List<OptimizationOptionDto>();
        public decimal FundingRequired { get; set; }
        public List<string> Recommendations { get; set; } = new List<string>();
    }

    /// <summary>
    /// DTO for optimization options
    /// </summary>
    public class OptimizationOptionDto
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public decimal Impact { get; set; }
        public decimal CostSavings { get; set; }
        public int AdditionalMonths { get; set; }
        public string Difficulty { get; set; }
    }
}
