using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Traceability.Enums;
using NouAssist.RequirementsManagement.Traceability.Constants;

namespace NouAssist.RequirementsManagement.Traceability.Artifact.Dtos
{
    /// <summary>
    /// DTO for registering an external artifact source
    /// </summary>
    public class RegisterArtifactSourceDto
    {
        /// <summary>
        /// Name of the artifact source
        /// </summary>
        [Required]
        [StringLength(TraceabilityManagementConsts.MaxArtifactNameLength)]
        public string SourceName { get; set; }
        
        /// <summary>
        /// Type of artifacts provided by this source
        /// </summary>
        [Required]
        public ArtifactType ArtifactType { get; set; }
        
        /// <summary>
        /// Base URI for accessing artifacts
        /// </summary>
        [Required]
        [StringLength(TraceabilityManagementConsts.MaxArtifactUriLength)]
        public string BaseUri { get; set; }
        
        /// <summary>
        /// Connection parameters for the artifact source
        /// </summary>
        public Dictionary<string, string> ConnectionParameters { get; set; } = new Dictionary<string, string>();
    }
}
