using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorProfiles.Dtos
{
    /// <summary>
    /// DTO for filtering and paging investor profile lists
    /// </summary>
    public class GetInvestorProfileListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public InvestorType? Type { get; set; }
        public InvestorStatus? Status { get; set; }
        public decimal? MinInvestmentAmount { get; set; }
        public decimal? MaxInvestmentAmount { get; set; }
        public int? MinRelationshipScore { get; set; }
        public int? MaxRelationshipScore { get; set; }
        public bool? HasRecentActivity { get; set; } // Last 90 days
        public string CompanyName { get; set; }
        public List<Guid> GroupIds { get; set; } = new();
    }
}
