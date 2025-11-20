using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.DueDiligenceDataRooms.Dtos
{
    /// <summary>
    /// DTO for filtering and paging data room lists
    /// </summary>
    public class GetDueDiligenceDataRoomListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public bool? IsActive { get; set; }
        public bool? RequirePasswordAccess { get; set; }
        public DateTime? ExpiringBefore { get; set; }
        public bool IncludeStatistics { get; set; } = true;
        public bool AccessibleOnly { get; set; } = false; // Show only accessible to current user
    }
}
