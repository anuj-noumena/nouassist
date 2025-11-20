namespace NouAssist.RequirementsManagement.Traceability.Events
{
    /// <summary>
    /// Event raised when a trace link is deleted
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.Traceability.TraceLinkDeleted")]
    public class TraceLinkDeletedEto
    {
        /// <summary>
        /// Unique identifier of the deleted trace link
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Source artifact identifier
        /// </summary>
        public string SourceArtifactId { get; set; }
        
        /// <summary>
        /// Target artifact identifier
        /// </summary>
        public string TargetArtifactId { get; set; }
        
        /// <summary>
        /// Tenant ID for multi-tenancy support
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
