namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Events.Compliance
{
    /// <summary>
    /// Event published when a compliance obligation status changes
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.LegalCompliance.ComplianceStatusChanged")]
    public class ComplianceStatusChangedEto : LegalEntityEtoBase
    {
        public string Title { get; set; }
        public ComplianceStatus PreviousStatus { get; set; }
        public ComplianceStatus NewStatus { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsOverdue { get; set; }
    }
}
