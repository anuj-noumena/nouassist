using NouAssist.StartupEntrepreneurship.CapitalizationTable.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Events
{
    /// <summary>
    /// Event published when a cap table is created
    /// </summary>
    [EventName("CapTable.Created")]
    public class CapTableCreatedEto : CapTableEtoBase
    {
        public string CompanyName { get; set; }
        public decimal TotalShares { get; set; }
    }
}
