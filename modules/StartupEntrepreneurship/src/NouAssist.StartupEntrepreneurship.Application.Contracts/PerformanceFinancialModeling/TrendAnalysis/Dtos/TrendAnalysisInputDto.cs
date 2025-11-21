using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.TrendAnalysis.Dtos
{
    /// <summary>
    /// Input DTO for trend analysis
    /// </summary>
    public class AnalyzeTrendDto
    {
        [Range(7, 365)]
        public int PeriodDays { get; set; } = 90;
    }

    /// <summary>
    /// Input DTO for pattern detection
    /// </summary>
    public class DetectPatternsDto
    {
        [Range(30, 730)]
        public int PeriodDays { get; set; } = 180;
    }

    /// <summary>
    /// Input DTO for forecasting
    /// </summary>
    public class ForecastMetricDto
    {
        [Range(1, 36)]
        public int ForecastPeriods { get; set; } = 12;

        public string Method { get; set; } = "LinearTrend";
    }

    /// <summary>
    /// Input DTO for seasonality analysis
    /// </summary>
    public class AnalyzeSeasonalityDto
    {
        [Range(90, 1095)]
        public int PeriodDays { get; set; } = 365;
    }

    /// <summary>
    /// Input DTO for correlation analysis
    /// </summary>
    public class CalculateCorrelationDto
    {
        [Required]
        public Guid Metric2Id { get; set; }

        [Range(30, 365)]
        public int PeriodDays { get; set; } = 90;
    }

    /// <summary>
    /// Input DTO for anomaly detection
    /// </summary>
    public class DetectAnomaliesDto
    {
        [Range(30, 365)]
        public int PeriodDays { get; set; } = 90;

        [Range(1.0, 5.0)]
        public decimal SensitivityThreshold { get; set; } = 2.0m;
    }
}
