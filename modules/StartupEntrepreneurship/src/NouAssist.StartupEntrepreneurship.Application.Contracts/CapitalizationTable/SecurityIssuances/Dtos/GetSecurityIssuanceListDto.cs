using System;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityIssuances.Dtos
{
    /// <summary>
    /// DTO for filtering SecurityIssuance lists
    /// </summary>
    public class GetSecurityIssuanceListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; } // General text filter
        public Guid? CapTableId { get; set; } // Filter by cap table
        public SecurityType? SecurityType { get; set; } // Filter by security type
        public bool? IsActive { get; set; } // Filter by active status
        public bool? HasAvailableShares { get; set; } // Filter by availability
    }
}
