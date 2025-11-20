namespace NouAssist.RequirementsManagement.Specification.Events
{
    /// <summary>
    /// Event published when a requirement is created
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.Specification.RequirementCreated")]
    public class RequirementCreatedEto : RequirementEtoBase
    {
        public Guid CreatorId { get; set; }
        public string Version { get; set; }
    }
}
