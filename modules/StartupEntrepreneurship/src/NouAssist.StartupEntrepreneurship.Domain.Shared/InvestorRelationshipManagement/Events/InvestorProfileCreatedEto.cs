namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Events
{
    /// <summary>
    /// Event published when a new investor profile is created
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorProfileCreated")]
    public class InvestorProfileCreatedEto : IRMEtoBase
    {
        public string InvestorName { get; set; }
        public string Email { get; set; }
        public InvestorType InvestorType { get; set; }
        public InvestorStatus Status { get; set; }
    }
}
