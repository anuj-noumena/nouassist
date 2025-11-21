using System;

namespace NouAssist.RequirementsManagement.ValidationVerification.Events.Base
{
    /// <summary>
    /// Base class for ValidationVerification events
    /// </summary>
    public abstract class ValidationVerificationEtoBase
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime EventTime { get; set; } = DateTime.UtcNow;
        public Guid EventId { get; set; } = Guid.NewGuid();
    }
}
