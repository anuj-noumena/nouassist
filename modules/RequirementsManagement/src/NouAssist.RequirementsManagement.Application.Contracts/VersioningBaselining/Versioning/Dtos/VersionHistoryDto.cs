using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Versioning.Dtos
{
    /// <summary>
    /// DTO representing the version history of a requirement
    /// </summary>
    public class VersionHistoryDto : EntityDto<Guid>
    {
        /// <summary>
        /// ID of the requirement
        /// </summary>
        public Guid RequirementId { get; set; }
        
        /// <summary>
        /// Latest version of the requirement
        /// </summary>
        public string LatestVersion { get; set; }
        
        /// <summary>
        /// Collection of version records
        /// </summary>
        public List<VersionRecordDto> Versions { get; set; }
    }
}
