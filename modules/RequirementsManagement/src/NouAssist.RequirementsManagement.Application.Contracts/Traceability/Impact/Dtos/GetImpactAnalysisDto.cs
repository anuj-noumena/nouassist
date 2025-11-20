using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Traceability.Enums;
using NouAssist.RequirementsManagement.Traceability.Constants;

namespace NouAssist.RequirementsManagement.Traceability.Impact.Dtos
{
    /// <summary>
    /// DTO for requesting an impact analysis
    /// </summary>
    public class GetImpactAnalysisDto
    {
        /// <summary>
        /// ID of the artifact to analyze
        /// </summary>
        [Required]
        [StringLength(TraceabilityManagementConsts.MaxArtifactIdLength)]
        public string ArtifactId { get; set; }
        
        /// <summary>
        /// Type of the artifact to analyze
        /// </summary>
        [Required]
        public ArtifactType ArtifactType { get; set; }
        
        /// <summary>
        /// Maximum depth for the impact analysis (1-5)
        /// </summary>
        [Range(1, 5)]
        public int MaxDepth { get; set; } = 2;
    }
}
