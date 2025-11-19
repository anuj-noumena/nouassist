using System.Collections.Generic;

namespace NouAssist.FinancialManagement.AuditTrail.Dtos
{
    public class EntityAuditHistoryDto
    {
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public string EntityName { get; set; }
        public List<AuditEntryDto> History { get; set; }
    }
}
