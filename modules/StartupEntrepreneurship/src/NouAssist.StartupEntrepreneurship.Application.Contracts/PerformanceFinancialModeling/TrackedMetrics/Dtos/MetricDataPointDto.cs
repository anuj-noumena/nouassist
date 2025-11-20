using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Dtos.Base;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.TrackedMetrics.Dtos
{
    /// <summary>
    /// DTO for MetricDataPoint
    /// </summary>
    public class MetricDataPointDto
    {
        public Guid MetricId { get; set; }
        public MetricValueDto Value { get; set; }
        public DateTime Timestamp { get; set; }
        public string Notes { get; set; }
    }

    /// <summary>
    /// Input DTO for bulk adding data points
    /// </summary>
    public class BulkAddDataPointsDto
    {
        public List<AddDataPointDto> DataPoints { get; set; } = new List<AddDataPointDto>();
    }

    /// <summary>
    /// Input DTO for getting data points in a period
    /// </summary>
    public class GetDataPointsDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? MaxCount { get; set; }
    }
}
