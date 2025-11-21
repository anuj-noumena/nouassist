using System.Collections.Generic;
using System.Linq;
using NouAssist.RequirementsManagement.Traceability.Enums;

namespace NouAssist.RequirementsManagement.Traceability.Models
{
    /// <summary>
    /// Represents the result of an impact analysis
    /// </summary>
    public class ImpactAnalysisResult
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
        public List<ImpactedArtifact> ImpactedArtifacts { get; private set; } = new List<ImpactedArtifact>();
        
        /// <summary>
        /// Adds an impacted artifact to the result
        /// </summary>
        public void AddImpactedArtifact(
            string artifactId,
            ArtifactType artifactType,
            LinkType linkType,
            LinkDirection direction,
            int impactDepth)
        {
            // Check if this artifact is already in the list
            var existing = ImpactedArtifacts.FirstOrDefault(a => 
                a.ArtifactId == artifactId && a.ArtifactType == artifactType);
                
            if (existing != null)
            {
                // If already in the list with a higher depth, update to the lower depth
                if (existing.ImpactDepth > impactDepth)
                {
                    existing.ImpactDepth = impactDepth;
                    existing.LinkType = linkType;
                    existing.LinkDirection = direction;
                }
            }
            else
            {
                // Add new impacted artifact
                ImpactedArtifacts.Add(new ImpactedArtifact
                {
                    ArtifactId = artifactId,
                    ArtifactType = artifactType,
                    LinkType = linkType,
                    LinkDirection = direction,
                    ImpactDepth = impactDepth
                });
            }
        }
        
        /// <summary>
        /// Gets impacted artifacts at a specific depth
        /// </summary>
        public List<ImpactedArtifact> GetImpactedArtifactsAtDepth(int depth)
        {
            return ImpactedArtifacts.Where(a => a.ImpactDepth == depth).ToList();
        }
        
        /// <summary>
        /// Gets impacted artifacts of a specific type
        /// </summary>
        public List<ImpactedArtifact> GetImpactedArtifactsByType(ArtifactType artifactType)
        {
            return ImpactedArtifacts.Where(a => a.ArtifactType == artifactType).ToList();
        }
    }
    
    /// <summary>
    /// Represents an artifact impacted by a change
    /// </summary>
    public class ImpactedArtifact
    {
        /// <summary>
        /// ID of the impacted artifact
        /// </summary>
        public string ArtifactId { get; set; }
        
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
