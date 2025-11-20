namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Events
{
    /// <summary>
    /// Event published when a goal target is set
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.StrategicPlanning.GoalTargetSet")]
    public class GoalTargetSetEto : StrategicPlanningEtoBase
    {
        public Guid GoalId { get; set; }
        public string MetricName { get; set; }
        public decimal TargetValue { get; set; }
        public string Unit { get; set; }
        public DateTime TargetDate { get; set; }
    }
}
