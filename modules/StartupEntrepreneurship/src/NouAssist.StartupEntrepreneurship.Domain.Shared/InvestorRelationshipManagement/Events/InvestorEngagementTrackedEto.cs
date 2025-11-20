namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Events
{
    /// <summary>
    /// Event published when investor engagement is tracked
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorEngagementTracked")]
    public class InvestorEngagementTrackedEto : IRMEtoBase
    {
        public Guid InvestorId { get; set; }
        public Guid UpdateId { get; set; }
        public EngagementType EngagementType { get; set; }
        public string Details { get; set; }
    }
}
