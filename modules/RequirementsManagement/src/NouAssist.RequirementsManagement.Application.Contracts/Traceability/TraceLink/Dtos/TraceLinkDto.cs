using System;
using System.Collections.Generic;
using NouAssist.RequirementsManagement.Traceability.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.Traceability.TraceLink.Dtos
{
    /// <summary>
    /// DTO for displaying trace link information
    /// </summary>
    public class TraceLinkDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// Source artifact in the trace relationship
        /// </summary>
        public TraceableArtifactDto SourceArtifact { get; set; }
        
        /// <summary>
        /// Target artifact in the trace relationship
        /// </summary>
        public TraceableArtifactDto TargetArtifact { get; set; }
        
        /// <summary>
        /// Type of relationship between the artifacts
        /// </summary>
        public LinkType LinkType { get; set; }
        
        /// <summary>
        /// Direction of the relationship between artifacts
        /// </summary>
        public LinkDirection LinkDirection { get; set; }
        
        /// <summary>
        /// Custom link type description when LinkType is Custom
        /// </summary>
        public string CustomLinkType { get; set; }
        
        /// <summary>
        /// Optional description of the trace link
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Unique textual identifier for this link
        /// </summary>
        public string Identifier { get; set; }
        
        /// <summary>
        /// Last time this link was verified by a user
        /// </summary>
        public DateTime? LastVerifiedTime { get; set; }
        
        /// <summary>
        /// User who last verified this link
        /// </summary>
        public Guid? LastVerifiedBy { get; set; }
        
        /// <summary>
        /// Indicates if this link is considered suspect and needs verification
        /// </summary>
        public bool IsSuspect { get; set; }
    }
}
