using NouAssist.RequirementsManagement.Traceability.Enums;

namespace NouAssist.RequirementsManagement.Traceability.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to create a duplicate trace link
    /// </summary>
    public class DuplicateLinkException : TraceabilityManagementDomainException
    {
        public string SourceArtifactId { get; }
        public string TargetArtifactId { get; }
        public LinkType LinkType { get; }

        public DuplicateLinkException(string sourceArtifactId, string targetArtifactId, LinkType linkType)
            : base("TraceabilityManagement:ErrorMessage:DuplicateLink")
        {
            SourceArtifactId = sourceArtifactId;
            TargetArtifactId = targetArtifactId;
            LinkType = linkType;
            
            WithData("sourceArtifactId", sourceArtifactId);
            WithData("targetArtifactId", targetArtifactId);
            WithData("linkType", linkType);
        }
    }
}
