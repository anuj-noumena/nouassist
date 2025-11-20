using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Traceability.Enums;
using NouAssist.RequirementsManagement.Traceability.Constants;

namespace NouAssist.RequirementsManagement.Traceability.TraceLink.Dtos
{
    /// <summary>
    /// DTO for updating an existing trace link
    /// </summary>
    public class UpdateTraceLinkDto
    {
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
