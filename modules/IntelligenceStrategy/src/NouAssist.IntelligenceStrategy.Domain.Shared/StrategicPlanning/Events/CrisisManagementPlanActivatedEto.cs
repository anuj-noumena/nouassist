namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Events
{
    /// <summary>
    /// Event published when a crisis management plan is activated
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.StrategicPlanning.CrisisManagementPlanActivated")]
    public class CrisisManagementPlanActivatedEto : StrategicPlanningEtoBase
    {
        public string PlanName { get; set; }
        public CrisisLevel Level { get; set; }
        public string Description { get; set; }
        public DateTime ActivatedAt { get; set; }
        public Guid ActivatedBy { get; set; }
    }
}
