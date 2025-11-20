namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.TrendAnalysis.Dtos
{
    /// <summary>
    /// DTO for trend analysis results
    /// </summary>
    public class TrendAnalysisDto
    {
        public Guid MetricId { get; set; }
        public string MetricName { get; set; }
        public string TrendDirection { get; set; }
        public decimal TrendStrength { get; set; }
        public decimal PercentageChange { get; set; }
        public decimal AverageGrowthRate { get; set; }
        public decimal Volatility { get; set; }
        public DateTime AnalysisDate { get; set; }
        public int DataPointsAnalyzed { get; set; }
        public string Confidence { get; set; }
    }

    /// <summary>
    /// DTO for trend patterns
    /// </summary>
    public class TrendPatternDto
    {
        public string PatternType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public decimal Confidence { get; set; }
    }

    /// <summary>
    /// DTO for forecast results
    /// </summary>
    public class ForecastResultDto
    {
        public Guid MetricId { get; set; }
        public string MetricName { get; set; }
        public List<ProjectedAmountDto> ForecastedValues { get; set; } = new List<ProjectedAmountDto>();
        public decimal Accuracy { get; set; }
        public string Method { get; set; }
        public DateTime ForecastDate { get; set; }
        public List<decimal> ConfidenceIntervals { get; set; } = new List<decimal>();
    }

    /// <summary>
    /// DTO for seasonality analysis
    /// </summary>
    public class SeasonalityAnalysisDto
    {
        public Guid MetricId { get; set; }
        public string MetricName { get; set; }
        public bool HasSeasonality { get; set; }
        public decimal SeasonalityStrength { get; set; }
        public Dictionary<int, decimal> MonthlyFactors { get; set; } = new Dictionary<int, decimal>();
        public string SeasonalPattern { get; set; }
        public DateTime AnalysisDate { get; set; }
    }

    /// <summary>
    /// DTO for correlation analysis
    /// </summary>
    public class CorrelationResultDto
    {
        public Guid Metric1Id { get; set; }
        public string Metric1Name { get; set; }
        public Guid Metric2Id { get; set; }
        public string Metric2Name { get; set; }
        public decimal CorrelationCoefficient { get; set; }
        public string CorrelationStrength { get; set; }
        public string Relationship { get; set; }
        public decimal Significance { get; set; }
        public DateTime AnalysisDate { get; set; }
    }

    /// <summary>
    /// DTO for anomaly detection
    /// </summary>
    public class AnomalyDto
    {
        public Guid MetricId { get; set; }
        public string MetricName { get; set; }
        public DateTime Date { get; set; }
        public decimal ActualValue { get; set; }
        public decimal ExpectedValue { get; set; }
        public decimal Deviation { get; set; }
        public string Severity { get; set; }
        public string PossibleCause { get; set; }
    }
}
