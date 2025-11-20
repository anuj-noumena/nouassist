namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Events
{
    /// <summary>
    /// Event raised when a metric goal is achieved
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.MetricGoalAchieved")]
    public class MetricGoalAchievedEto : PerformanceFinancialModelingEtoBase
    {
        public Guid MetricId { get; set; }
        public string MetricName { get; set; }
        public Guid GoalId { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal TargetValue { get; set; }
    }
}
