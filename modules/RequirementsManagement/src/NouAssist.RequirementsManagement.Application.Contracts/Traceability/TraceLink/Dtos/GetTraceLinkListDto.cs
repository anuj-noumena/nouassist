using NouAssist.RequirementsManagement.Traceability.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.Traceability.TraceLink.Dtos
{
    /// <summary>
    /// DTO for filtering and paging trace link list
    /// </summary>
    public class GetTraceLinkListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text to filter link descriptions or artifact names
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Filter for links of specific type
        /// </summary>
        public LinkType? LinkType { get; set; }
        
        /// <summary>
        /// Filter for links with specific direction
        /// </summary>
        public LinkDirection? LinkDirection { get; set; }
        
        /// <summary>
        /// Filter for links with a specific source artifact ID
        /// </summary>
        public string SourceArtifactId { get; set; }
        
        /// <summary>
        /// Filter for links with a specific source artifact type
        /// </summary>
        public ArtifactType? SourceArtifactType { get; set; }
        
        /// <summary>
        /// Filter for links with a specific target artifact ID
        /// </summary>
        public string TargetArtifactId { get; set; }
        
        /// <summary>
        /// Filter for links with a specific target artifact type
        /// </summary>
        public ArtifactType? TargetArtifactType { get; set; }
        
        /// <summary>
        /// Filter to only show suspect links that need verification
        /// </summary>
        public bool? OnlySuspect { get; set; }
    }
}
