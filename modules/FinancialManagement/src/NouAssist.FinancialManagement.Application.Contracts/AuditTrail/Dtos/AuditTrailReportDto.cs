using System;
using System.Collections.Generic;

namespace NouAssist.AuditTrail.Dtos
{
    public class AuditTrailReportDto
    {
        public DateTime GeneratedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public AuditTrailSummaryDto Summary { get; set; }
        public List<AuditEntryDto> Entries { get; set; }
    }

    public class AuditTrailSummaryDto
    {
        public long TotalEntries { get; set; }
        public int UniqueUsers { get; set; }
        public int EntitiesModified { get; set; }
        public int HighRiskOperations { get; set; }
        public int SuspiciousActivity { get; set; }
    }
}
