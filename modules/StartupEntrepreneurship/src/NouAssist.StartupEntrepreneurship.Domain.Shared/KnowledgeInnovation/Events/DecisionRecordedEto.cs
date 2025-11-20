namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Events
{
    /// <summary>
    /// Event raised when a decision is recorded
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.KnowledgeInnovation.DecisionRecorded")]
    public class DecisionRecordedEto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DecisionStatus Status { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime RecordedAt { get; set; }
        public Guid RecordedBy { get; set; }
    }
}
