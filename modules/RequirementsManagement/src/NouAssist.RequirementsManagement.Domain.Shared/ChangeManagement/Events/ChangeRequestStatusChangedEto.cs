namespace NouAssist.RequirementsManagement.ChangeManagement.Events
{
    /// <summary>
    /// Event published when a change request status changes
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.ChangeManagement.ChangeRequestStatusChanged")]
    public class ChangeRequestStatusChangedEto : ChangeRequestEtoBase
    {
        public RequestStatus PreviousStatus { get; set; }
        public Guid? ChangedByUserId { get; set; }
        public string ChangeReason { get; set; }
    }
}
