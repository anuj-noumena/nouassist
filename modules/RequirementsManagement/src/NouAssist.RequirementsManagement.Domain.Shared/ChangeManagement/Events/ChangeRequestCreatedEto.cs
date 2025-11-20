namespace NouAssist.RequirementsManagement.ChangeManagement.Events
{
    /// <summary>
    /// Event published when a change request is created
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.ChangeManagement.ChangeRequestCreated")]
    public class ChangeRequestCreatedEto : ChangeRequestEtoBase
    {
        public Guid SubmitterId { get; set; }
        public AffectedRequirementRef AffectedRequirement { get; set; }
    }
}
