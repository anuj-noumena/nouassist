namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Events
{
    /// <summary>
    /// Event published when a relationship is established between requirements
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.AnalysisPrioritization.RequirementRelationshipEstablished")]
    public class RequirementRelationshipEstablishedEto
    {
        public Guid SourceRequirementId { get; set; }
        public Guid TargetRequirementId { get; set; }
        public RelationshipType RelationshipType { get; set; }
        public string? Description { get; set; }
        public Guid? TenantId { get; set; }
    }
}
