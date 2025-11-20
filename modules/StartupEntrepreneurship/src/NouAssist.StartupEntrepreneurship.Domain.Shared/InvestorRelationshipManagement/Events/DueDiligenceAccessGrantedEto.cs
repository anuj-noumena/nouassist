namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Events
{
    /// <summary>
    /// Event published when due diligence access is granted
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.DueDiligenceAccessGranted")]
    public class DueDiligenceAccessGrantedEto : IRMEtoBase
    {
        public Guid InvestorId { get; set; }
        public Guid DataRoomId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
