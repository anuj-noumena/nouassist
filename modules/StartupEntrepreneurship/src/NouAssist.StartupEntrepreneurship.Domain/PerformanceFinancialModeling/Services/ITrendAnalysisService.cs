namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Services
{
    /// <summary>
    /// Service for analyzing metric trends and patterns
    /// </summary>
    public interface ITrendAnalysisService
    {
        Task<TrendAnalysis> AnalyzeTrendAsync(TrackedMetric metric, int periodDays = 90);
        Task<List<TrendPattern>> DetectPatternsAsync(TrackedMetric metric, int periodDays = 180);
        Task<ForecastResult> ForecastMetricAsync(TrackedMetric metric, int forecastPeriods = 12);
        Task<SeasonalityAnalysis> AnalyzeSeasonalityAsync(TrackedMetric metric, int periodDays = 365);
        Task<CorrelationResult> CalculateCorrelationAsync(TrackedMetric metric1, TrackedMetric metric2, int periodDays = 90);
        Task<List<Anomaly>> DetectAnomaliesAsync(TrackedMetric metric, int periodDays = 90);
    }

    public class TrendAnalysis
    {
        public string TrendDirection { get; set; }
        public decimal TrendStrength { get; set; }
        public decimal PercentageChange { get; set; }
        public decimal AverageGrowthRate { get; set; }
        public decimal Volatility { get; set; }
        public DateTime AnalysisDate { get; set; }
        public int DataPointsAnalyzed { get; set; }
        public string Confidence { get; set; }
    }

    public class TrendPattern
    {
        public string PatternType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public decimal Confidence { get; set; }
    }

    public class ForecastResult
    {
        public List<ProjectedAmount> ForecastedValues { get; set; }
        public decimal Accuracy { get; set; }
        public string Method { get; set; }
        public DateTime ForecastDate { get; set; }
        public List<decimal> ConfidenceIntervals { get; set; }
    }

    public class SeasonalityAnalysis
    {
        public bool HasSeasonality { get; set; }
        public decimal SeasonalityStrength { get; set; }
        public Dictionary<int, decimal> MonthlyFactors { get; set; }
        public string SeasonalPattern { get; set; }
        public DateTime AnalysisDate { get; set; }
    }

    public class CorrelationResult
    {
        public decimal CorrelationCoefficient { get; set; }
        public string CorrelationStrength { get; set; }
        public string Relationship { get; set; }
        public decimal Significance { get; set; }
        public DateTime AnalysisDate { get; set; }
    }

    public class Anomaly
    {
        public DateTime Date { get; set; }
        public decimal ActualValue { get; set; }
        public decimal ExpectedValue { get; set; }
        public decimal Deviation { get; set; }
        public string Severity { get; set; }
        public string PossibleCause { get; set; }
    }
}
