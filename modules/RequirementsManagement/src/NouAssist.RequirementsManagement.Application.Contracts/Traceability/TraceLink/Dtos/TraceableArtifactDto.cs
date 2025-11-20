using System;
using NouAssist.RequirementsManagement.Traceability.Enums;

namespace NouAssist.RequirementsManagement.Traceability.TraceLink.Dtos
{
    /// <summary>
    /// DTO for displaying traceable artifact information
    /// </summary>
    public class TraceableArtifactDto
    {
        /// <summary>
        /// Unique ID of the artifact
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Type of the artifact
        /// </summary>
        public ArtifactType Type { get; set; }
        
        /// <summary>
        /// Version of the artifact, if applicable
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// URI to access the artifact, if applicable
        /// </summary>
        public string Uri { get; set; }
        
        /// <summary>
        /// Display name for the artifact
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Last verified timestamp
        /// </summary>
        public DateTime? LastVerified { get; set; }
        
        /// <summary>
        /// Last modified timestamp from the source system
        /// </summary>
        public DateTime? LastModified { get; set; }
        
        /// <summary>
        /// Indicates if this artifact reference is considered suspect and needs verification
        /// </summary>
        public bool IsSuspect { get; set; }
    }
}
