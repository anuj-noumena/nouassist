using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Traceability.Enums;

namespace NouAssist.RequirementsManagement.Traceability.TraceMatrix.Dtos
{
    /// <summary>
    /// DTO for requesting a traceability matrix
    /// </summary>
    public class GetTraceabilityMatrixDto
    {
        /// <summary>
        /// Type of source artifacts
        /// </summary>
        [Required]
        public ArtifactType SourceType { get; set; }
        
        /// <summary>
        /// Type of target artifacts
        /// </summary>
        [Required]
        public ArtifactType TargetType { get; set; }
        
        /// <summary>
        /// Type of links to include in the matrix (optional filter)
        /// </summary>
        public LinkType? LinkType { get; set; }
    }
}
