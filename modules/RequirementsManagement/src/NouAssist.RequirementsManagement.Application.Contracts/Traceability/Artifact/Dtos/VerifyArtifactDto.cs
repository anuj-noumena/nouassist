using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Traceability.Enums;
using NouAssist.RequirementsManagement.Traceability.Constants;

namespace NouAssist.RequirementsManagement.Traceability.Artifact.Dtos
{
    /// <summary>
    /// DTO for verifying if an artifact exists
    /// </summary>
    public class VerifyArtifactDto
    {
        /// <summary>
        /// ID of the artifact to verify
        /// </summary>
        [Required]
        [StringLength(TraceabilityManagementConsts.MaxArtifactIdLength)]
        public string ArtifactId { get; set; }
        
        /// <summary>
        /// Type of the artifact to verify
        /// </summary>
        [Required]
        public ArtifactType ArtifactType { get; set; }
        
        /// <summary>
        /// Version of the artifact, if applicable
        /// </summary>
        [StringLength(TraceabilityManagementConsts.MaxArtifactVersionLength)]
        public string Version { get; set; }
    }
}
