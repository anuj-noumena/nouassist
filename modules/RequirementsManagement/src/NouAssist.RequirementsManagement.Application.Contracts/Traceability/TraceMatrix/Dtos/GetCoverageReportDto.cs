using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Traceability.Enums;

namespace NouAssist.RequirementsManagement.Traceability.TraceMatrix.Dtos
{
    /// <summary>
    /// DTO for requesting a coverage report
    /// </summary>
    public class GetCoverageReportDto
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
        /// Type of links to include in the coverage analysis
        /// </summary>
        [Required]
        public LinkType LinkType { get; set; }
    }
}
