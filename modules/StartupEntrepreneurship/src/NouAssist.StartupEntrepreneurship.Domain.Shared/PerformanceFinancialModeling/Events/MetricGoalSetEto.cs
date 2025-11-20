namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Events
{
    /// <summary>
    /// Event raised when a goal is set for a metric
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.MetricGoalSet")]
    public class MetricGoalSetEto : PerformanceFinancialModelingEtoBase
    {
        public Guid MetricId { get; set; }
        public string MetricName { get; set; }
        public decimal TargetValue { get; set; }
        public DateTime TargetDate { get; set; }
        public string Description { get; set; }
    }
}
