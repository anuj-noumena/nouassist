using System;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.FundingRounds.Dtos
{
    /// <summary>
    /// DTO for filtering FundingRound lists
    /// </summary>
    public class GetFundingRoundListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; } // General text filter
        public Guid? CapTableId { get; set; } // Filter by cap table
        public FundingRoundStatus? Status { get; set; } // Filter by status
        public DateTime? StartDateFrom { get; set; } // Filter by start date range
        public DateTime? StartDateTo { get; set; }
    }
}
