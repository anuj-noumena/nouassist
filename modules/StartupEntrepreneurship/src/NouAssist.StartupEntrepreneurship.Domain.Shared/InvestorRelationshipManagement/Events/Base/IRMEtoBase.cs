using System;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Events.Base
{
    /// <summary>
    /// Base class for IRM integration events
    /// </summary>
    [Serializable]
    public abstract class IRMEtoBase
    {
        public Guid Id { get; set; }
        public DateTime EventTime { get; set; } = DateTime.UtcNow;
        public Guid? TenantId { get; set; }
    }
}
