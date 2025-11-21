using System;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Events.Agreements
{
    /// <summary>
    /// Event published when a new legal agreement is created
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.LegalCompliance.LegalAgreementCreated")]
    public class LegalAgreementCreatedEto : LegalEntityEtoBase
    {
        public string Title { get; set; }
        public LegalAgreementType Type { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string CounterpartyName { get; set; }
        public FilingStatusType Status { get; set; }
    }
}
