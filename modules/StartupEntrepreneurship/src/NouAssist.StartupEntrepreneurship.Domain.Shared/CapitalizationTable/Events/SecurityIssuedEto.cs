using System;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Events.Base;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Events
{
    /// <summary>
    /// Event published when securities are issued
    /// </summary>
    [EventName("Security.Issued")]
    public class SecurityIssuedEto : CapTableEtoBase
    {
        public Guid CapTableId { get; set; }
        public Guid SecurityHolderId { get; set; }
        public SecurityType SecurityType { get; set; }
        public decimal Quantity { get; set; }
        public decimal? PricePerShare { get; set; }
    }
}
