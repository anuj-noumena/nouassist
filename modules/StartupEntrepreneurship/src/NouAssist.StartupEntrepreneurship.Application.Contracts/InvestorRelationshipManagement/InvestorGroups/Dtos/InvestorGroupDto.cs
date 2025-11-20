using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorGroups.Dtos
{
    /// <summary>
    /// DTO representing an investor group for read operations
    /// </summary>
    public class InvestorGroupDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int InvestorCount { get; set; }
        public List<Guid> InvestorIds { get; set; } = new();
        
        // Extended information (populated when requested)
        public List<InvestorGroupMemberDto> Investors { get; set; } = new();
    }
}
