using System;
using System.Collections.Generic;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.DueDiligenceDataRooms.Dtos
{
    /// <summary>
    /// DTO representing document access summary for analytics
    /// </summary>
    public class DocumentAccessSummaryDto
    {
        public Guid DocumentId { get; set; }
        public string DocumentName { get; set; }
        public int TotalAccesses { get; set; }
        public int UniqueViewers { get; set; }
        public DateTime LastAccessed { get; set; }
        public List<InvestorAccessSummaryDto> InvestorAccesses { get; set; } = new();
    }

    public class InvestorAccessSummaryDto
    {
        public Guid InvestorId { get; set; }
        public string InvestorName { get; set; }
        public int AccessCount { get; set; }
        public DateTime LastAccessed { get; set; }
        public List<string> Actions { get; set; } = new();
    }
}
