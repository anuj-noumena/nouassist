using System;
using NouAssist.RequirementsManagement.Traceability.Enums;

namespace NouAssist.RequirementsManagement.Traceability.Events.Base
{
    /// <summary>
    /// Base event transfer object for TraceLink events
    /// </summary>
    [Serializable]
    public abstract class TraceLinkEtoBase
    {
        /// <summary>
        /// Unique identifier of the trace link
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Source artifact identifier
        /// </summary>
        public string SourceArtifactId { get; set; }
        
        /// <summary>
        /// Target artifact identifier
        /// </summary>
        public string TargetArtifactId { get; set; }
        
        /// <summary>
        /// Type of link relationship
        /// </summary>
        public LinkType LinkType { get; set; }
        
        /// <summary>
        /// Direction of link relationship
        /// </summary>
        public LinkDirection LinkDirection { get; set; }
        
        /// <summary>
        /// Tenant ID for multi-tenancy support
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
