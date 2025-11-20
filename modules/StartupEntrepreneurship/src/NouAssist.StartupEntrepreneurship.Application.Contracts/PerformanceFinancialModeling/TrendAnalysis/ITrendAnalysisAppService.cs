using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.TrendAnalysis.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.TrendAnalysis
{
    /// <summary>
    /// Application service for trend analysis and forecasting
    /// </summary>
    public interface ITrendAnalysisAppService : IApplicationService
    {
        // Trend analysis
        Task<TrendAnalysisDto> AnalyzeTrendAsync(Guid metricId, AnalyzeTrendDto input);

        // Pattern detection
        Task<List<TrendPatternDto>> DetectPatternsAsync(Guid metricId, DetectPatternsDto input);

        // Forecasting
        Task<ForecastResultDto> ForecastMetricAsync(Guid metricId, ForecastMetricDto input);

        // Seasonality analysis
        Task<SeasonalityAnalysisDto> AnalyzeSeasonalityAsync(Guid metricId, AnalyzeSeasonalityDto input);

        // Correlation analysis
        Task<CorrelationResultDto> CalculateCorrelationAsync(Guid metricId, CalculateCorrelationDto input);

        // Anomaly detection
        Task<List<AnomalyDto>> DetectAnomaliesAsync(Guid metricId, DetectAnomaliesDto input);

        // Bulk analysis
        Task<List<TrendAnalysisDto>> AnalyzeAllMetricsAsync();
        Task<List<AnomalyDto>> DetectAllAnomaliesAsync();
    }
}
