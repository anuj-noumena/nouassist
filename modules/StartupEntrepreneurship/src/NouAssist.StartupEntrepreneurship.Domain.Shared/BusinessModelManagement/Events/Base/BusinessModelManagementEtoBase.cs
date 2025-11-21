using System;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Events.Base
{
    /// <summary>
    /// Base event transfer object for Business Model Management events
    /// </summary>
    public abstract class BusinessModelManagementEtoBase
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime EventTime { get; set; } = DateTime.UtcNow;
    }
}
