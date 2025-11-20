using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Baselining.Dtos
{
    /// <summary>
    /// DTO representing an item in a baseline
    /// </summary>
    public class BaselineItemDto : EntityDto
    {
        /// <summary>
        /// ID of the requirement
        /// </summary>
        public Guid RequirementId { get; set; }
        
        /// <summary>
        /// Version of the requirement
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// Timestamp when this item was added to the baseline
        /// </summary>
        public DateTime AddedAt { get; set; }
    }
}
