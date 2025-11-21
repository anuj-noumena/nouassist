using System;

namespace NouAssist.RequirementsManagement.Elicitation.Events.Base
{
    /// <summary>
    /// Base class for all Elicitation domain events
    /// </summary>
    [Serializable]
    public abstract class ElicitationEtoBase
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime EventTime { get; set; } = DateTime.UtcNow;
    }
}
