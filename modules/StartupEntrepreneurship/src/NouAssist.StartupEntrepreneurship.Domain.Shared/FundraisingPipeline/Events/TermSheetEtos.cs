using System;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Events
{
    /// <summary>
    /// Event raised when a new term sheet is received
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.FundraisingPipeline.TermSheetReceived")]
    public class TermSheetReceivedEto : FundraisingEtoBase
    {
        public Guid ProspectId { get; set; }
        public string ProspectName { get; set; }
        public string ProspectCompany { get; set; }
        public decimal OfferedAmount { get; set; }
        public decimal Valuation { get; set; }
    }
}
