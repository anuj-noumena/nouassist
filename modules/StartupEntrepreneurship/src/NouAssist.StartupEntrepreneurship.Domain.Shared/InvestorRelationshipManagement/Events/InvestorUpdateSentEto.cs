using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Events
{
    /// <summary>
    /// Event published when investor update is sent
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorUpdateSent")]
    public class InvestorUpdateSentEto : IRMEtoBase
    {
        public string Title { get; set; }
        public List<Guid> InvestorIds { get; set; } = new();
        public DateTime ScheduledDate { get; set; }
        public UpdateFrequency Frequency { get; set; }
    }
}
