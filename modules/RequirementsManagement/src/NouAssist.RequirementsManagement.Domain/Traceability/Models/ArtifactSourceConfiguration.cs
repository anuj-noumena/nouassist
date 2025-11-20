namespace NouAssist.RequirementsManagement.Traceability.Models
{
    /// <summary>
    /// Configuration for an external artifact source
    /// </summary>
    public class ArtifactSourceConfiguration
    {
        /// <summary>
        /// Name of the artifact source
        /// </summary>
        public string SourceName { get; set; }
        
        /// <summary>
        /// Type of artifacts provided by this source
        /// </summary>
        public ArtifactType ArtifactType { get; set; }
        
        /// <summary>
        /// Base URI for accessing artifacts
        /// </summary>
        public string BaseUri { get; set; }
        
        /// <summary>
        /// Connection parameters for the artifact source
        /// </summary>
        public Dictionary<string, string> ConnectionParameters { get; set; }
        
        /// <summary>
        /// When this source was registered
        /// </summary>
        public DateTime RegisteredAt { get; set; }
    }
}
