using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityHolders.Dtos
{
    /// <summary>
    /// DTO for filtering SecurityHolder lists
    /// </summary>
    public class GetSecurityHolderListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; } // General text filter
        public HolderType? HolderType { get; set; } // Filter by holder type
        public bool? IsActive { get; set; } // Filter by active status
    }
}
