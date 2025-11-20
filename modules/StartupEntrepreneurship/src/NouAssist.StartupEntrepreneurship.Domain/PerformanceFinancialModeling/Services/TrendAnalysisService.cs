namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Services
{
    /// <summary>
    /// Domain service for trend analysis and forecasting
    /// </summary>
    public class TrendAnalysisService : DomainService, ITrendAnalysisService
    {
        private readonly ILogger<TrendAnalysisService> _logger;

        public TrendAnalysisService(ILogger<TrendAnalysisService> logger)
        {
            _logger = logger;
        }

        public async Task<TrendAnalysis> AnalyzeTrendAsync(TrackedMetric metric, int periodDays = 90)
        {
            await Task.CompletedTask;

            var cutoffDate = DateTime.UtcNow.AddDays(-periodDays);
            var dataPoints = metric.GetDataPointsInPeriod(cutoffDate, DateTime.UtcNow);

            if (dataPoints.Count < 2)
            {
                return new TrendAnalysis
                {
                    TrendDirection = "Insufficient Data",
                    TrendStrength = 0,
                    PercentageChange = 0,
                    AverageGrowthRate = 0,
                    Volatility = 0,
                    AnalysisDate = DateTime.UtcNow,
                    DataPointsAnalyzed = dataPoints.Count,
                    Confidence = "Low"
                };
            }

            var values = dataPoints.Select(dp => dp.Value.Value).ToList();
            var firstValue = values.First();
            var lastValue = values.Last();
            
            var percentageChange = firstValue != 0 ? ((lastValue - firstValue) / firstValue) * 100 : 0;
            var averageGrowthRate = CalculateAverageGrowthRate(values);
            var volatility = CalculateVolatility(values);
            var trendStrength = CalculateTrendStrength(values);
            
            var trendDirection = GetTrendDirection(percentageChange, trendStrength);
            var confidence = GetConfidence(dataPoints.Count, volatility);

            return new TrendAnalysis
            {
                TrendDirection = trendDirection,
                TrendStrength = trendStrength,
                PercentageChange = percentageChange,
                AverageGrowthRate = averageGrowthRate,
                Volatility = volatility,
                AnalysisDate = DateTime.UtcNow,
                DataPointsAnalyzed = dataPoints.Count,
                Confidence = confidence
            };
        }

        public async Task<List<TrendPattern>> DetectPatternsAsync(TrackedMetric metric, int periodDays = 180)
        {
            await Task.CompletedTask;

            var patterns = new List<TrendPattern>();
            var cutoffDate = DateTime.UtcNow.AddDays(-periodDays);
            var dataPoints = metric.GetDataPointsInPeriod(cutoffDate, DateTime.UtcNow);

            if (dataPoints.Count < 10) return patterns;

            // Detect growth/decline patterns
            patterns.AddRange(DetectGrowthDeclinePatterns(dataPoints));
            
            // Detect plateau patterns
            patterns.AddRange(DetectPlateauPatterns(dataPoints));
            
            // Detect cyclical patterns
            patterns.AddRange(DetectCyclicalPatterns(dataPoints));

            return patterns;
        }

        public async Task<ForecastResult> ForecastMetricAsync(TrackedMetric metric, int forecastPeriods = 12)
        {
            await Task.CompletedTask;

            var dataPoints = metric.GetDataPointsInPeriod(DateTime.UtcNow.AddDays(-365), DateTime.UtcNow);
            
            if (dataPoints.Count < 3)
            {
                return new ForecastResult
                {
                    ForecastedValues = new List<ProjectedAmount>(),
                    Accuracy = 0,
                    Method = "Insufficient Data",
                    ForecastDate = DateTime.UtcNow,
                    ConfidenceIntervals = new List<decimal>()
                };
            }

            // Simple linear trend forecasting
            var values = dataPoints.Select(dp => dp.Value.Value).ToList();
            var trend = CalculateLinearTrend(values);
            var lastValue = values.Last();
            var baseDate = dataPoints.Last().Timestamp;

            var forecastedValues = new List<ProjectedAmount>();
            for (int i = 1; i <= forecastPeriods; i++)
            {
                var projectedValue = lastValue + (trend * i);
                var projectionDate = baseDate.AddMonths(i);
                var confidence = Math.Max(0.5m, 1m - (i * 0.05m)); // Decreasing confidence over time
                
                var lowerBound = projectedValue * (1 - (1 - confidence));
                var upperBound = projectedValue * (1 + (1 - confidence));
                
                forecastedValues.Add(new ProjectedAmount(projectedValue, i, projectionDate, confidence, lowerBound, upperBound));
            }

            var accuracy = CalculateForecastAccuracy(dataPoints);

            return new ForecastResult
            {
                ForecastedValues = forecastedValues,
                Accuracy = accuracy,
                Method = "Linear Trend",
                ForecastDate = DateTime.UtcNow,
                ConfidenceIntervals = forecastedValues.Select(f => f.ConfidenceLevel * 100).ToList()
            };
        }

        public async Task<SeasonalityAnalysis> AnalyzeSeasonalityAsync(TrackedMetric metric, int periodDays = 365)
        {
            await Task.CompletedTask;

            var cutoffDate = DateTime.UtcNow.AddDays(-periodDays);
            var dataPoints = metric.GetDataPointsInPeriod(cutoffDate, DateTime.UtcNow);

            var monthlyFactors = new Dictionary<int, decimal>();
            var monthlyData = new Dictionary<int, List<decimal>>();

            // Group data by month
            foreach (var dataPoint in dataPoints)
            {
                var month = dataPoint.Timestamp.Month;
                if (!monthlyData.ContainsKey(month))
                {
                    monthlyData[month] = new List<decimal>();
                }
                monthlyData[month].Add(dataPoint.Value.Value);
            }

            // Calculate monthly factors
            var overallAverage = dataPoints.Average(dp => dp.Value.Value);
            foreach (var month in monthlyData.Keys)
            {
                var monthlyAverage = monthlyData[month].Average();
                monthlyFactors[month] = overallAverage != 0 ? monthlyAverage / overallAverage : 1;
            }

            // Determine seasonality strength
            var seasonalityStrength = CalculateSeasonalityStrength(monthlyFactors.Values);
            var hasSeasonality = seasonalityStrength > 0.1m; // 10% threshold

            return new SeasonalityAnalysis
            {
                HasSeasonality = hasSeasonality,
                SeasonalityStrength = seasonalityStrength,
                MonthlyFactors = monthlyFactors,
                SeasonalPattern = DetermineSeasonalPattern(monthlyFactors),
                AnalysisDate = DateTime.UtcNow
            };
        }

        public async Task<CorrelationResult> CalculateCorrelationAsync(TrackedMetric metric1, TrackedMetric metric2, int periodDays = 90)
        {
            await Task.CompletedTask;

            var cutoffDate = DateTime.UtcNow.AddDays(-periodDays);
            var data1 = metric1.GetDataPointsInPeriod(cutoffDate, DateTime.UtcNow);
            var data2 = metric2.GetDataPointsInPeriod(cutoffDate, DateTime.UtcNow);

            // Align data points by date
            var alignedData = AlignDataPointsByDate(data1, data2);
            
            if (alignedData.Count < 3)
            {
                return new CorrelationResult
                {
                    CorrelationCoefficient = 0,
                    CorrelationStrength = "Insufficient Data",
                    Relationship = "Unknown",
                    Significance = 0,
                    AnalysisDate = DateTime.UtcNow
                };
            }

            var correlationCoefficient = CalculatePearsonCorrelation(alignedData);
            var correlationStrength = GetCorrelationStrength(Math.Abs(correlationCoefficient));
            var relationship = GetRelationshipType(correlationCoefficient);
            var significance = CalculateSignificance(correlationCoefficient, alignedData.Count);

            return new CorrelationResult
            {
                CorrelationCoefficient = correlationCoefficient,
                CorrelationStrength = correlationStrength,
                Relationship = relationship,
                Significance = significance,
                AnalysisDate = DateTime.UtcNow
            };
        }

        public async Task<List<Anomaly>> DetectAnomaliesAsync(TrackedMetric metric, int periodDays = 90)
        {
            await Task.CompletedTask;

            var anomalies = new List<Anomaly>();
            var cutoffDate = DateTime.UtcNow.AddDays(-periodDays);
            var dataPoints = metric.GetDataPointsInPeriod(cutoffDate, DateTime.UtcNow);

            if (dataPoints.Count < 5) return anomalies;

            var values = dataPoints.Select(dp => dp.Value.Value).ToList();
            var average = values.Average();
            var standardDeviation = CalculateStandardDeviation(values);

            // Detect outliers using z-score method
            for (int i = 0; i < dataPoints.Count; i++)
            {
                var value = dataPoints[i].Value.Value;
                var zScore = standardDeviation != 0 ? Math.Abs((value - average) / standardDeviation) : 0;

                if (zScore > 2) // Values beyond 2 standard deviations
                {
                    anomalies.Add(new Anomaly
                    {
                        Date = dataPoints[i].Timestamp,
                        ActualValue = value,
                        ExpectedValue = average,
                        Deviation = Math.Abs(value - average),
                        Severity = zScore > 3 ? "High" : "Medium",
                        PossibleCause = DeterminePossibleCause(value, average, dataPoints[i])
                    });
                }
            }

            return anomalies;
        }

        // Helper methods
        private decimal CalculateAverageGrowthRate(List<decimal> values)
        {
            if (values.Count < 2) return 0;

            var growthRates = new List<decimal>();
            for (int i = 1; i < values.Count; i++)
            {
                if (values[i - 1] != 0)
                {
                    growthRates.Add((values[i] - values[i - 1]) / values[i - 1]);
                }
            }

            return growthRates.Any() ? growthRates.Average() * 100 : 0;
        }

        private decimal CalculateVolatility(List<decimal> values)
        {
            if (values.Count < 2) return 0;

            var average = values.Average();
            var squaredDifferences = values.Select(v => (v - average) * (v - average));
            var variance = squaredDifferences.Average();
            
            return (decimal)Math.Sqrt((double)variance);
        }

        private decimal CalculateTrendStrength(List<decimal> values)
        {
            if (values.Count < 3) return 0;

            // Calculate R-squared for linear trend
            var n = values.Count;
            var sumX = 0;
            var sumY = values.Sum();
            var sumXY = 0m;
            var sumXX = 0;

            for (int i = 0; i < n; i++)
            {
                sumX += i;
                sumXY += i * values[i];
                sumXX += i * i;
            }

            var slope = (n * sumXY - sumX * sumY) / (n * sumXX - sumX * sumX);
            var intercept = (sumY - slope * sumX) / n;

            var totalSumSquares = values.Sum(v => (v - values.Average()) * (v - values.Average()));
            var residualSumSquares = 0m;

            for (int i = 0; i < n; i++)
            {
                var predicted = slope * i + intercept;
                residualSumSquares += (values[i] - predicted) * (values[i] - predicted);
            }

            return totalSumSquares != 0 ? 1 - (residualSumSquares / totalSumSquares) : 0;
        }

        private string GetTrendDirection(decimal percentageChange, decimal trendStrength)
        {
            if (trendStrength < 0.1m) return "Stable";
            if (percentageChange > 5) return "Strong Upward";
            if (percentageChange > 0) return "Upward";
            if (percentageChange < -5) return "Strong Downward";
            if (percentageChange < 0) return "Downward";
            return "Stable";
        }

        private string GetConfidence(int dataPointCount, decimal volatility)
        {
            if (dataPointCount < 5) return "Low";
            if (dataPointCount < 10 || volatility > 0.5m) return "Medium";
            return "High";
        }

        private List<TrendPattern> DetectGrowthDeclinePatterns(List<MetricDataPoint> dataPoints)
        {
            var patterns = new List<TrendPattern>();
            // Simplified pattern detection - could be enhanced with more sophisticated algorithms
            return patterns;
        }

        private List<TrendPattern> DetectPlateauPatterns(List<MetricDataPoint> dataPoints)
        {
            var patterns = new List<TrendPattern>();
            // Simplified pattern detection
            return patterns;
        }

        private List<TrendPattern> DetectCyclicalPatterns(List<MetricDataPoint> dataPoints)
        {
            var patterns = new List<TrendPattern>();
            // Simplified pattern detection
            return patterns;
        }

        private decimal CalculateLinearTrend(List<decimal> values)
        {
            if (values.Count < 2) return 0;

            var n = values.Count;
            var sumX = 0;
            var sumY = values.Sum();
            var sumXY = 0m;
            var sumXX = 0;

            for (int i = 0; i < n; i++)
            {
                sumX += i;
                sumXY += i * values[i];
                sumXX += i * i;
            }

            return (n * sumXY - sumX * sumY) / (n * sumXX - sumX * sumX);
        }

        private decimal CalculateForecastAccuracy(List<MetricDataPoint> historicalData)
        {
            // Simplified accuracy calculation - could use holdout validation
            return 0.85m; // 85% default accuracy
        }

        private decimal CalculateSeasonalityStrength(IEnumerable<decimal> monthlyFactors)
        {
            var factors = monthlyFactors.ToList();
            if (!factors.Any()) return 0;

            var average = factors.Average();
            var variance = factors.Sum(f => (f - average) * (f - average)) / factors.Count;
            
            return variance; // Simplified calculation
        }

        private string DetermineSeasonalPattern(Dictionary<int, decimal> monthlyFactors)
        {
            // Simplified pattern determination
            var maxMonth = monthlyFactors.OrderByDescending(kv => kv.Value).First().Key;
            var minMonth = monthlyFactors.OrderBy(kv => kv.Value).First().Key;

            return $"Peak in {GetMonthName(maxMonth)}, Low in {GetMonthName(minMonth)}";
        }

        private List<(DateTime date, decimal value1, decimal value2)> AlignDataPointsByDate(
            List<MetricDataPoint> data1, List<MetricDataPoint> data2)
        {
            var aligned = new List<(DateTime, decimal, decimal)>();
            
            foreach (var point1 in data1)
            {
                var point2 = data2.FirstOrDefault(p => p.Timestamp.Date == point1.Timestamp.Date);
                if (point2 != null)
                {
                    aligned.Add((point1.Timestamp, point1.Value.Value, point2.Value.Value));
                }
            }

            return aligned;
        }

        private decimal CalculatePearsonCorrelation(List<(DateTime date, decimal value1, decimal value2)> alignedData)
        {
            if (alignedData.Count < 2) return 0;

            var values1 = alignedData.Select(d => d.value1).ToList();
            var values2 = alignedData.Select(d => d.value2).ToList();

            var mean1 = values1.Average();
            var mean2 = values2.Average();

            var numerator = alignedData.Sum(d => (d.value1 - mean1) * (d.value2 - mean2));
            var denominator1 = (decimal)Math.Sqrt((double)alignedData.Sum(d => (d.value1 - mean1) * (d.value1 - mean1)));
            var denominator2 = (decimal)Math.Sqrt((double)alignedData.Sum(d => (d.value2 - mean2) * (d.value2 - mean2)));

            return denominator1 * denominator2 != 0 ? numerator / (denominator1 * denominator2) : 0;
        }

        private string GetCorrelationStrength(decimal correlation)
        {
            if (correlation >= 0.7m) return "Strong";
            if (correlation >= 0.3m) return "Moderate";
            if (correlation >= 0.1m) return "Weak";
            return "Very Weak";
        }

        private string GetRelationshipType(decimal correlation)
        {
            if (correlation > 0.1m) return "Positive";
            if (correlation < -0.1m) return "Negative";
            return "No Linear Relationship";
        }

        private decimal CalculateSignificance(decimal correlation, int sampleSize)
        {
            // Simplified significance calculation
            return Math.Abs(correlation) * sampleSize / 10;
        }

        private decimal CalculateStandardDeviation(List<decimal> values)
        {
            if (!values.Any()) return 0;

            var average = values.Average();
            var squaredDifferences = values.Select(v => (v - average) * (v - average));
            var variance = squaredDifferences.Average();
            
            return (decimal)Math.Sqrt((double)variance);
        }

        private string DeterminePossibleCause(decimal value, decimal average, MetricDataPoint dataPoint)
        {
            if (value > average * 1.5m) return "Unusually high value - possible data entry error or exceptional event";
            if (value < average * 0.5m) return "Unusually low value - possible system issue or market change";
            return "Statistical outlier";
        }

        private string GetMonthName(int month)
        {
            return new DateTime(2000, month, 1).ToString("MMMM");
        }
    }
}
