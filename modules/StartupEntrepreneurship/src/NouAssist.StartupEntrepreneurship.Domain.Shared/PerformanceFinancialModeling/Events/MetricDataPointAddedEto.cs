namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Events
{
    /// <summary>
    /// Event raised when a new data point is added to a metric
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.MetricDataPointAdded")]
    public class MetricDataPointAddedEto : PerformanceFinancialModelingEtoBase
    {
        public Guid MetricId { get; set; }
        public string MetricName { get; set; }
        public decimal Value { get; set; }
        public DateTime Timestamp { get; set; }
        public string Notes { get; set; }
    }
}
