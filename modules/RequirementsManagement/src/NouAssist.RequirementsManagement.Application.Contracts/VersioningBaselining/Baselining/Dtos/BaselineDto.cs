using NouAssist.RequirementsManagement.VersioningBaselining.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Baselining.Dtos
{
    /// <summary>
    /// DTO representing a baseline
    /// </summary>
    public class BaselineDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// Name of the baseline
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the baseline
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Status of the baseline
        /// </summary>
        public BaselineStatus Status { get; set; }
        
        /// <summary>
        /// Collection of items in the baseline
        /// </summary>
        public List<BaselineItemDto> Items { get; set; }
    }
}
