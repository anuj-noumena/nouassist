namespace NouAssist.RequirementsManagement.Traceability.Exceptions
{
    /// <summary>
    /// Exception thrown when an artifact is not found
    /// </summary>
    public class ArtifactNotFoundException : TraceabilityManagementDomainException
    {
        public string ArtifactId { get; }
        
        public ArtifactNotFoundException(string artifactId)
            : base("TraceabilityManagement:ErrorMessage:ArtifactNotFound")
        {
            ArtifactId = artifactId;
            WithData("artifactId", artifactId);
        }
    }
}
