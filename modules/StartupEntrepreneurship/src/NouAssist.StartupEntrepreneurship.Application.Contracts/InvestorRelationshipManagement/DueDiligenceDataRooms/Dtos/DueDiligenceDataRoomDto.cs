using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.DueDiligenceDataRooms.Dtos
{
    /// <summary>
    /// DTO representing a due diligence data room for read operations
    /// </summary>
    public class DueDiligenceDataRoomDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool RequirePasswordAccess { get; set; }
        public bool HasPasswordAccess { get; set; } // Indicates if current user can access

        // Statistics
        public int DocumentCount { get; set; }
        public int InvestorCount { get; set; }
        public long TotalDocumentSize { get; set; }
        public DateTime? LastAccessDate { get; set; }

        // Collections (populated based on request)
        public List<SecureDocumentDto> Documents { get; set; } = new();
        public List<DataRoomInvestorAccessDto> InvestorAccess { get; set; } = new();
    }
}
