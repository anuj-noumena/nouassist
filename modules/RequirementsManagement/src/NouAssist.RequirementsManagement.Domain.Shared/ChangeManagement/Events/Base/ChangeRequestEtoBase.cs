namespace NouAssist.RequirementsManagement.ChangeManagement.Events.Base
{
    /// <summary>
    /// Base class for change request events
    /// </summary>
    public abstract class ChangeRequestEtoBase
    {
        public Guid Id { get; set; }
        public string RequestIdentifier { get; set; }
        public string Title { get; set; }
        public RequestStatus Status { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime EventTime { get; set; } = DateTime.UtcNow;
    }
}
