using System;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Events.Base
{
    /// <summary>
    /// Base class for all Legal & Compliance domain events
    /// </summary>
    [Serializable]
    public abstract class LegalEntityEtoBase
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
