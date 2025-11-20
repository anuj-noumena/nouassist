using System.Collections.Generic;
using NouAssist.RequirementsManagement.Traceability.Enums;

namespace NouAssist.RequirementsManagement.Traceability.Impact.Dtos
{
    /// <summary>
    /// DTO for displaying impact analysis results
    /// </summary>
    public class ImpactAnalysisResultDto
    {
        /// <summary>
        /// ID of the source artifact for the impact analysis
        /// </summary>
        public string SourceArtifactId { get; set; }
        
        /// <summary>
        /// Type of the source artifact
        /// </summary>
        public ArtifactType SourceArtifactType { get; set; }
        
        /// <summary>
        /// List of impacted artifacts
        /// </summary>
        public List<ImpactedArtifactDto> ImpactedArtifacts { get; set; } = new List<ImpactedArtifactDto>();
        
        /// <summary>
        /// Summary of impacts by artifact type
        /// </summary>
        public Dictionary<ArtifactType, int> ImpactSummaryByType { get; set; } = new Dictionary<ArtifactType, int>();
        
        /// <summary>
        /// Summary of impacts by depth level
        /// </summary>
        public Dictionary<int, int> ImpactSummaryByDepth { get; set; } = new Dictionary<int, int>();
    }
    
    /// <summary>
    /// DTO for an artifact impacted by a change
    /// </summary>
    public class ImpactedArtifactDto
    {
        /// <summary>
        /// ID of the impacted artifact
        /// </summary>
        public string ArtifactId { get; set; }
        
        /// <summary>
        /// Name of the impacted artifact (if available)
        /// </summary>
        public string ArtifactName { get; set; }
        
        /// <summary>
        /// Type of the impacted artifact
        /// </summary>
        public ArtifactType ArtifactType { get; set; }
        
        /// <summary>
        /// Type of link that caused the impact
        /// </summary>
        public LinkType LinkType { get; set; }
        
        /// <summary>
        /// Direction of the link that caused the impact
        /// </summary>
        public LinkDirection LinkDirection { get; set; }
        
        /// <summary>
        /// Depth of the impact (distance from source artifact)
        /// </summary>
        public int ImpactDepth { get; set; }
    }
}
