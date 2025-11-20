using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Events.Base
{
    /// <summary>
    /// Base class for cap table domain events
    /// </summary>
    [GenericEventName(Prefix = "NouAssist.StartupEntrepreneurship.CapitalizationTable.")]
    public abstract class CapTableEtoBase
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime EventTime { get; set; } = DateTime.UtcNow;
    }
}
