namespace NouAssist.RequirementsManagement.Elicitation.Events
{
    /// <summary>
    /// Event published when an elicitation artifact is created
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.Elicitation.ElicitationArtifactCreated")]
    public class ElicitationArtifactCreatedEto : ElicitationEtoBase
    {
        public string Title { get; set; }
        public ArtifactType Type { get; set; }
        public Guid SessionId { get; set; }
        public Guid CreatedByStakeholderId { get; set; }
    }
}
