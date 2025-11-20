using NouAssist.StartupEntrepreneurship.CapitalizationTable.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Events
{
    /// <summary>
    /// Event published when shares are transferred
    /// </summary>
    [EventName("Shares.Transferred")]
    public class SharesTransferredEto : CapTableEtoBase
    {
        public Guid CapTableId { get; set; }
        public Guid FromHolderId { get; set; }
        public Guid ToHolderId { get; set; }
        public decimal Quantity { get; set; }
        public decimal? PricePerShare { get; set; }
    }
}
