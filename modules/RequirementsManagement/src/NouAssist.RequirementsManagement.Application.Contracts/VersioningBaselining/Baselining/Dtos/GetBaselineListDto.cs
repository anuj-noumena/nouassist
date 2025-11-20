using NouAssist.RequirementsManagement.VersioningBaselining.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Baselining.Dtos
{
    /// <summary>
    /// DTO for getting a list of baselines
    /// </summary>
    public class GetBaselineListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Filter by baseline name
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Filter by baseline status
        /// </summary>
        public BaselineStatus? Status { get; set; }
    }
}
