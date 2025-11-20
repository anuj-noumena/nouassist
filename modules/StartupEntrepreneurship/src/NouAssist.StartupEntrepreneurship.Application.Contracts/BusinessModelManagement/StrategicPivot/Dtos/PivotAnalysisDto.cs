using System.Collections.Generic;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.StrategicPivot.Dtos
{
    public class PivotAnalysisDto
    {
        public int TotalMetricsDefined { get; set; }
        public int MetricsWithData { get; set; }
        public double OverallSuccessRate { get; set; }
        public string PivotStatus { get; set; }
        public bool IsOverdue { get; set; }
        public double? Duration { get; set; }
        public List<MetricAnalysisDto> MetricAnalysis { get; set; }

        public PivotAnalysisDto()
        {
            MetricAnalysis = new List<MetricAnalysisDto>();
        }
    }

    public class MetricAnalysisDto
    {
        public string MetricName { get; set; }
        public decimal? BaselineValue { get; set; }
        public decimal? TargetValue { get; set; }
        public decimal? LatestValue { get; set; }
        public double? ProgressToTarget { get; set; }
        public string TrendDirection { get; set; }
        public int MeasurementCount { get; set; }
    }
}
