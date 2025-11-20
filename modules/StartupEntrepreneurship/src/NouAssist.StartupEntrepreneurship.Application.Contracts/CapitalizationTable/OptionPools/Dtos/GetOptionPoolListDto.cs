using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.OptionPools.Dtos
{
    /// <summary>
    /// DTO for filtering OptionPool lists
    /// </summary>
    public class GetOptionPoolListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; } // General text filter
        public Guid? CapTableId { get; set; } // Filter by cap table
        public bool? IsActive { get; set; } // Filter by active status
        public bool? HasAvailableOptions { get; set; } // Filter by availability
    }
}
