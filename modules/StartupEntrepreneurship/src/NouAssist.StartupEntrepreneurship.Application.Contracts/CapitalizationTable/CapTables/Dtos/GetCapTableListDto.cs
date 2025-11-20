using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.CapTables.Dtos
{
    /// <summary>
    /// DTO for filtering CapTable lists
    /// </summary>
    public class GetCapTableListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; } // General text filter for company name
        public bool? IsActive { get; set; } // Filter by active status
    }
}
