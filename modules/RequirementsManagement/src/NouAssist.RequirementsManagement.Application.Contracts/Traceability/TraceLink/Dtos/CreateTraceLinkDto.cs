using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Traceability.Enums;
using NouAssist.RequirementsManagement.Traceability.Constants;

namespace NouAssist.RequirementsManagement.Traceability.TraceLink.Dtos
{
    /// <summary>
    /// DTO for creating a new trace link
    /// </summary>
    public class CreateTraceLinkDto
    {
        /// <summary>
        /// Source artifact ID
        /// </summary>
        [Required]
        [StringLength(TraceabilityManagementConsts.MaxArtifactIdLength)]
        public string SourceArtifactId { get; set; }
        
        /// <summary>
        /// Source artifact type
        /// </summary>
        [Required]
        public ArtifactType SourceArtifactType { get; set; }
        
        /// <summary>
        /// Source artifact name
        /// </summary>
        [Required]
        [StringLength(TraceabilityManagementConsts.MaxArtifactNameLength)]
        public string SourceArtifactName { get; set; }
        
        /// <summary>
        /// Source artifact version (optional)
        /// </summary>
        [StringLength(TraceabilityManagementConsts.MaxArtifactVersionLength)]
        public string SourceArtifactVersion { get; set; }
        
        /// <summary>
        /// Source artifact URI (optional)
        /// </summary>
        [StringLength(TraceabilityManagementConsts.MaxArtifactUriLength)]
        public string SourceArtifactUri { get; set; }
        
        /// <summary>
        /// Target artifact ID
        /// </summary>
        [Required]
        [StringLength(TraceabilityManagementConsts.MaxArtifactIdLength)]
        public string TargetArtifactId { get; set; }
        
        /// <summary>
        /// Target artifact type
        /// </summary>
        [Required]
        public ArtifactType TargetArtifactType { get; set; }
        
        /// <summary>
        /// Target artifact name
        /// </summary>
        [Required]
        [StringLength(TraceabilityManagementConsts.MaxArtifactNameLength)]
        public string TargetArtifactName { get; set; }
        
        /// <summary>
        /// Target artifact version (optional)
        /// </summary>
        [StringLength(TraceabilityManagementConsts.MaxArtifactVersionLength)]
        public string TargetArtifactVersion { get; set; }
        
        /// <summary>
        /// Target artifact URI (optional)
        /// </summary>
        [StringLength(TraceabilityManagementConsts.MaxArtifactUriLength)]
        public string TargetArtifactUri { get; set; }
        
        /// <summary>
        /// Type of relationship between the artifacts
        /// </summary>
        [Required]
        public LinkType LinkType { get; set; }
        
        /// <summary>
        /// Direction of the relationship between artifacts
        /// </summary>
        [Required]
        public LinkDirection LinkDirection { get; set; }
        
        /// <summary>
        /// Custom link type description (required when LinkType is Custom)
        /// </summary>
        [StringLength(TraceabilityManagementConsts.MaxCustomLinkTypeLength)]
        public string CustomLinkType { get; set; }
        
        /// <summary>
        /// Optional description of the trace link
        /// </summary>
        [StringLength(TraceabilityManagementConsts.MaxLinkDescriptionLength)]
        public string Description { get; set; }
    }
}
