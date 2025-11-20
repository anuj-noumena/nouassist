namespace NouAssist.RequirementsManagement.Traceability.Models
{
    /// <summary>
    /// Represents a traceability matrix between source and target artifacts
    /// </summary>
    public class TraceabilityMatrix
    {
        /// <summary>
        /// Type of source artifacts
        /// </summary>
        public ArtifactType SourceType { get; set; }
        
        /// <summary>
        /// Type of target artifacts
        /// </summary>
        public ArtifactType TargetType { get; set; }
        
        /// <summary>
        /// Type of links included in the matrix, if filtered
        /// </summary>
        public LinkType? LinkType { get; set; }
        
        /// <summary>
        /// List of source artifact IDs
        /// </summary>
        public List<string> SourceArtifacts { get; private set; } = new List<string>();
        
        /// <summary>
        /// List of target artifact IDs
        /// </summary>
        public List<string> TargetArtifacts { get; private set; } = new List<string>();
        
        /// <summary>
        /// Matrix of links from source to target artifacts
        /// Dictionary structure: source ID -> target ID -> (link type, link ID)
        /// </summary>
        public Dictionary<string, Dictionary<string, List<(LinkType Type, Guid LinkId)>>> Links { get; private set; } 
            = new Dictionary<string, Dictionary<string, List<(LinkType, Guid)>>>();
            
        /// <summary>
        /// Adds a link to the matrix
        /// </summary>
        public void AddLink(string sourceId, string targetId, LinkType linkType, Guid linkId)
        {
            if (!Links.ContainsKey(sourceId))
            {
                Links[sourceId] = new Dictionary<string, List<(LinkType, Guid)>>();
            }
            
            if (!Links[sourceId].ContainsKey(targetId))
            {
                Links[sourceId][targetId] = new List<(LinkType, Guid)>();
            }
            
            Links[sourceId][targetId].Add((linkType, linkId));
        }
        
        /// <summary>
        /// Gets links between specific source and target artifacts
        /// </summary>
        public List<(LinkType Type, Guid LinkId)> GetLinks(string sourceId, string targetId)
        {
            if (Links.TryGetValue(sourceId, out var targetDict) && targetDict.TryGetValue(targetId, out var links))
            {
                return links;
            }
            
            return new List<(LinkType, Guid)>();
        }
        
        /// <summary>
        /// Checks if a link exists between specific source and target artifacts
        /// </summary>
        public bool HasLink(string sourceId, string targetId)
        {
            return Links.TryGetValue(sourceId, out var targetDict) && targetDict.ContainsKey(targetId);
        }
    }
}
