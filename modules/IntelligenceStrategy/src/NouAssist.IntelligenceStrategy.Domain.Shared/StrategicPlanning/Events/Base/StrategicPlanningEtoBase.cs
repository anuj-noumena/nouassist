namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Events.Base
{
    /// <summary>
    /// Base class for Strategic Planning events
    /// </summary>
    [Serializable]
    public abstract class StrategicPlanningEtoBase
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
    }
}
