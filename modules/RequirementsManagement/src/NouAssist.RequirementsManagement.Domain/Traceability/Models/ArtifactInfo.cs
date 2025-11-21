using System;

namespace NouAssist.RequirementsManagement.Traceability.Models
{
    /// <summary>
    /// Represents information about an artifact from an external system
    /// </summary>
    public class ArtifactInfo
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
        /// Display name of the artifact
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Version of the artifact, if applicable
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// Last modification time of the artifact
        /// </summary>
        public DateTime LastModified { get; set; }
        
        /// <summary>
        /// URI to access the artifact
        /// </summary>
        public string Uri { get; set; }
    }
}
