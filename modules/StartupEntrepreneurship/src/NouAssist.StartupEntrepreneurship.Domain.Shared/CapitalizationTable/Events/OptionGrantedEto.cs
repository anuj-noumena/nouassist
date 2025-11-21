using System;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Events
{
    /// <summary>
    /// Event published when stock options are granted
    /// </summary>
    [EventName("Option.Granted")]
    public class OptionGrantedEto : CapTableEtoBase
    {
        public Guid CapTableId { get; set; }
        public Guid SecurityHolderId { get; set; }
        public decimal Quantity { get; set; }
        public decimal StrikePrice { get; set; }
        public DateTime GrantDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
