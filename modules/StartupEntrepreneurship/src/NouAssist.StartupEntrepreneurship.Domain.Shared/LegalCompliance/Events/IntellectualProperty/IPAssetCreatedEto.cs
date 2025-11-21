using System;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Events.IntellectualProperty
{
    /// <summary>
    /// Event published when a new intellectual property asset is created
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.LegalCompliance.IPAssetCreated")]
    public class IPAssetCreatedEto : LegalEntityEtoBase
    {
        public string Name { get; set; }
        public IntellectualPropertyType Type { get; set; }
        public string Identifier { get; set; }
        public DateTime FilingDate { get; set; }
        public FilingStatusType Status { get; set; }
        public string Jurisdiction { get; set; }
    }
}
