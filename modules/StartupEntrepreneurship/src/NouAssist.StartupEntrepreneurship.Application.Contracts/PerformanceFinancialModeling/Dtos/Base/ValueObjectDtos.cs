using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Dtos.Base
{
    /// <summary>
    /// DTO for MetricValue value object
    /// </summary>
    public class MetricValueDto
    {
        public decimal Value { get; set; }
        public MetricDataType DataType { get; set; }
        public string Unit { get; set; }
    }

    /// <summary>
    /// DTO for CalculationRule value object
    /// </summary>
    public class CalculationRuleDto
    {
        public string Formula { get; set; }
        public string Description { get; set; }
        public List<string> RequiredMetrics { get; set; } = new List<string>();
        public bool IsAutomated { get; set; }
    }

    /// <summary>
    /// DTO for GoalTarget value object
    /// </summary>
    public class GoalTargetDto
    {
        public MetricValueDto TargetValue { get; set; }
        public DateTime TargetDate { get; set; }
        public ThresholdType ThresholdType { get; set; }
        public MetricValueDto SecondaryValue { get; set; }
    }

    /// <summary>
    /// DTO for AssumptionValue value object
    /// </summary>
    public class AssumptionValueDto
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
    }

    /// <summary>
    /// DTO for ProjectedAmount value object
    /// </summary>
    public class ProjectedAmountDto
    {
        public decimal Amount { get; set; }
        public decimal? LowerBound { get; set; }
        public decimal? UpperBound { get; set; }
        public decimal ConfidenceLevel { get; set; }
        public int PeriodIndex { get; set; }
        public DateTime ProjectionDate { get; set; }
    }

    /// <summary>
    /// DTO for BurnRateValue value object
    /// </summary>
    public class BurnRateValueDto
    {
        public decimal MonthlyAmount { get; set; }
        public decimal DailyAmount { get; set; }
        public DateTime CalculationDate { get; set; }
        public int PeriodMonths { get; set; }
    }

    /// <summary>
    /// DTO for RunwayLength value object
    /// </summary>
    public class RunwayLengthDto
    {
        public int Months { get; set; }
        public DateTime DepletionDate { get; set; }
        public decimal CurrentBurnRate { get; set; }
        public decimal CurrentCashBalance { get; set; }
    }

    /// <summary>
    /// DTO for BreakEvenResult value object
    /// </summary>
    public class BreakEvenResultDto
    {
        public DateTime? BreakEvenDate { get; set; }
        public int? BreakEvenMonthIndex { get; set; }
        public decimal BreakEvenRevenue { get; set; }
        public decimal BreakEvenUnits { get; set; }
        public decimal FixedCosts { get; set; }
        public decimal VariableCostPerUnit { get; set; }
        public decimal PricePerUnit { get; set; }
    }
}
