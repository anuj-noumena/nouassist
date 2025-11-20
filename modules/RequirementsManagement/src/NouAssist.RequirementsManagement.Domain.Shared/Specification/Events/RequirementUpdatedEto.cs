namespace NouAssist.RequirementsManagement.Specification.Events
{
    /// <summary>
    /// Event published when a requirement is updated
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.Specification.RequirementUpdated")]
    public class RequirementUpdatedEto : RequirementEtoBase
    {
        public Guid ModifierId { get; set; }
        public string PreviousVersion { get; set; }
        public string NewVersion { get; set; }
    }
}
