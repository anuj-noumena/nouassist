using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Permissions;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Repositories;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Services;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.TrendAnalysis.Dtos;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.TrendAnalysis
{
    [Authorize(PerformanceFinancialModelingPermissions.Analytics.Default)]
    public class TrendAnalysisAppService : ApplicationService, ITrendAnalysisAppService
    {
        private readonly ITrackedMetricRepository _trackedMetricRepository;
        private readonly ITrendAnalysisService _trendAnalysisService;
        private readonly ICurrentTenant _currentTenant;

        public TrendAnalysisAppService(
            ITrackedMetricRepository trackedMetricRepository,
            ITrendAnalysisService trendAnalysisService,
            ICurrentTenant currentTenant)
        {
            _trackedMetricRepository = trackedMetricRepository;
            _trendAnalysisService = trendAnalysisService;
            _currentTenant = currentTenant;

            LocalizationResource = typeof(PerformanceFinancialModelingResource);
        }

        [Authorize(PerformanceFinancialModelingPermissions.Analytics.ViewTrends)]
        public virtual async Task<TrendAnalysisDto> AnalyzeTrendAsync(Guid metricId, AnalyzeTrendDto input)
        {
            try
            {
                var metric = await _trackedMetricRepository.GetAsync(metricId, includeDetails: true);
                CheckResourceAccess(metric);

                var analysis = await _trendAnalysisService.AnalyzeTrendAsync(metric, input.PeriodDays);

                return new TrendAnalysisDto
                {
                    MetricId = metricId,
                    MetricName = metric.Name,
                    TrendDirection = analysis.TrendDirection,
                    TrendStrength = analysis.TrendStrength,
                    PercentageChange = analysis.PercentageChange,
                    AverageGrowthRate = analysis.AverageGrowthRate,
                    Volatility = analysis.Volatility,
                    AnalysisDate = analysis.AnalysisDate,
                    DataPointsAnalyzed = analysis.DataPointsAnalyzed,
                    Confidence = analysis.Confidence
                };
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:MetricNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.Analytics.ViewTrends)]
        public virtual async Task<List<TrendPatternDto>> DetectPatternsAsync(Guid metricId, DetectPatternsDto input)
        {
            try
            {
                var metric = await _trackedMetricRepository.GetAsync(metricId, includeDetails: true);
                CheckResourceAccess(metric);

                var patterns = await _trendAnalysisService.DetectPatternsAsync(metric, input.PeriodDays);

                return patterns.Select(p => new TrendPatternDto
                {
                    PatternType = p.PatternType,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Description = p.Description,
                    Confidence = p.Confidence
                }).ToList();
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:MetricNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.Analytics.ViewTrends)]
        public virtual async Task<ForecastResultDto> ForecastMetricAsync(Guid metricId, ForecastMetricDto input)
        {
            try
            {
                var metric = await _trackedMetricRepository.GetAsync(metricId, includeDetails: true);
                CheckResourceAccess(metric);

                var forecast = await _trendAnalysisService.ForecastMetricAsync(metric, input.ForecastPeriods);

                return new ForecastResultDto
                {
                    MetricId = metricId,
                    MetricName = metric.Name,
                    ForecastedValues = ObjectMapper.Map<List<ProjectedAmount>, List<ProjectedAmountDto>>(forecast.ForecastedValues),
                    Accuracy = forecast.Accuracy,
                    Method = forecast.Method,
                    ForecastDate = forecast.ForecastDate,
                    ConfidenceIntervals = forecast.ConfidenceIntervals
                };
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:MetricNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.Analytics.ViewTrends)]
        public virtual async Task<SeasonalityAnalysisDto> AnalyzeSeasonalityAsync(Guid metricId, AnalyzeSeasonalityDto input)
        {
            try
            {
                var metric = await _trackedMetricRepository.GetAsync(metricId, includeDetails: true);
                CheckResourceAccess(metric);

                var seasonality = await _trendAnalysisService.AnalyzeSeasonalityAsync(metric, input.PeriodDays);

                return new SeasonalityAnalysisDto
                {
                    MetricId = metricId,
                    MetricName = metric.Name,
                    HasSeasonality = seasonality.HasSeasonality,
                    SeasonalityStrength = seasonality.SeasonalityStrength,
                    MonthlyFactors = seasonality.MonthlyFactors,
                    SeasonalPattern = seasonality.SeasonalPattern,
                    AnalysisDate = seasonality.AnalysisDate
                };
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:MetricNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.Analytics.ViewTrends)]
        public virtual async Task<CorrelationResultDto> CalculateCorrelationAsync(Guid metricId, CalculateCorrelationDto input)
        {
            try
            {
                var metric1 = await _trackedMetricRepository.GetAsync(metricId, includeDetails: true);
                CheckResourceAccess(metric1);

                var metric2 = await _trackedMetricRepository.GetAsync(input.Metric2Id, includeDetails: true);
                CheckResourceAccess(metric2);

                var correlation = await _trendAnalysisService.CalculateCorrelationAsync(metric1, metric2, input.PeriodDays);

                return new CorrelationResultDto
                {
                    Metric1Id = metricId,
                    Metric1Name = metric1.Name,
                    Metric2Id = input.Metric2Id,
                    Metric2Name = metric2.Name,
                    CorrelationCoefficient = correlation.CorrelationCoefficient,
                    CorrelationStrength = correlation.CorrelationStrength,
                    Relationship = correlation.Relationship,
                    Significance = correlation.Significance,
                    AnalysisDate = correlation.AnalysisDate
                };
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:MetricNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.Analytics.ViewTrends)]
        public virtual async Task<List<AnomalyDto>> DetectAnomaliesAsync(Guid metricId, DetectAnomaliesDto input)
        {
            try
            {
                var metric = await _trackedMetricRepository.GetAsync(metricId, includeDetails: true);
                CheckResourceAccess(metric);

                var anomalies = await _trendAnalysisService.DetectAnomaliesAsync(metric, input.PeriodDays);

                return anomalies.Select(a => new AnomalyDto
                {
                    MetricId = metricId,
                    MetricName = metric.Name,
                    Date = a.Date,
                    ActualValue = a.ActualValue,
                    ExpectedValue = a.ExpectedValue,
                    Deviation = a.Deviation,
                    Severity = a.Severity,
                    PossibleCause = a.PossibleCause
                }).ToList();
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ErrorMessage:MetricNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.Analytics.ViewTrends)]
        public virtual async Task<List<TrendAnalysisDto>> AnalyzeAllMetricsAsync()
        {
            try
            {
                var metrics = await _trackedMetricRepository.GetActiveMetricsAsync(_currentTenant.Id);
                var analyses = new List<TrendAnalysisDto>();

                foreach (var metric in metrics)
                {
                    try
                    {
                        var analysis = await _trendAnalysisService.AnalyzeTrendAsync(metric, 90);
                        analyses.Add(new TrendAnalysisDto
                        {
                            MetricId = metric.Id,
                            MetricName = metric.Name,
                            TrendDirection = analysis.TrendDirection,
                            TrendStrength = analysis.TrendStrength,
                            PercentageChange = analysis.PercentageChange,
                            AverageGrowthRate = analysis.AverageGrowthRate,
                            Volatility = analysis.Volatility,
                            AnalysisDate = analysis.AnalysisDate,
                            DataPointsAnalyzed = analysis.DataPointsAnalyzed,
                            Confidence = analysis.Confidence
                        });
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWarning(ex, "Failed to analyze trend for metric {MetricId}", metric.Id);
                    }
                }

                return analyses;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(PerformanceFinancialModelingPermissions.Analytics.ViewTrends)]
        public virtual async Task<List<AnomalyDto>> DetectAllAnomaliesAsync()
        {
            try
            {
                var metrics = await _trackedMetricRepository.GetActiveMetricsAsync(_currentTenant.Id);
                var allAnomalies = new List<AnomalyDto>();

                foreach (var metric in metrics)
                {
                    try
                    {
                        var anomalies = await _trendAnalysisService.DetectAnomaliesAsync(metric, 90);
                        var anomalyDtos = anomalies.Select(a => new AnomalyDto
                        {
                            MetricId = metric.Id,
                            MetricName = metric.Name,
                            Date = a.Date,
                            ActualValue = a.ActualValue,
                            ExpectedValue = a.ExpectedValue,
                            Deviation = a.Deviation,
                            Severity = a.Severity,
                            PossibleCause = a.PossibleCause
                        }).ToList();

                        allAnomalies.AddRange(anomalyDtos);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWarning(ex, "Failed to detect anomalies for metric {MetricId}", metric.Id);
                    }
                }

                // Sort by severity and date
                return allAnomalies
                    .OrderBy(a => a.Severity == "High" ? 1 : a.Severity == "Medium" ? 2 : 3)
                    .ThenByDescending(a => a.Date)
                    .ToList();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:DefaultErrorMessage"]);
            }
        }

        // Helper methods
        protected virtual void CheckResourceAccess(TrackedMetric metric)
        {
            if (metric.TenantId != _currentTenant.Id)
            {
                throw new EntityNotFoundException(typeof(TrackedMetric), metric.Id);
            }
        }

        protected virtual async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in TrendAnalysisAppService: {ErrorMessage}", exception.Message);
            await Task.CompletedTask;
        }
    }
}
