namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Events
{
    /// <summary>
    /// Event published when a transition plan is activated
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.StrategicPlanning.TransitionPlanActivated")]
    public class TransitionPlanActivatedEto : StrategicPlanningEtoBase
    {
        public string PlanName { get; set; }
        public TransitionType Type { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime EstimatedCompletionDate { get; set; }
        public decimal EstimatedCost { get; set; }
    }
}
