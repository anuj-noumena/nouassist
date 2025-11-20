using System;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorUpdates.Dtos
{
    /// <summary>
    /// DTO for filtering and paging investor update lists
    /// </summary>
    public class GetInvestorUpdateListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public UpdateStatus? Status { get; set; }
        public UpdateFrequency? Frequency { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string TemplateId { get; set; }
        public bool IncludeStatistics { get; set; } = true;
    }
}
