using NouAssist.StartupEntrepreneurship.CapitalizationTable.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Events
{
    /// <summary>
    /// Event published when a funding round is closed
    /// </summary>
    [EventName("FundingRound.Closed")]
    public class FundingRoundClosedEto : CapTableEtoBase
    {
        public Guid CapTableId { get; set; }
        public string RoundName { get; set; }
        public decimal TotalRaised { get; set; }
        public decimal PreMoneyValuation { get; set; }
        public decimal PostMoneyValuation { get; set; }
    }
}
