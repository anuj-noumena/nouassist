using System;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Events
{
    /// <summary>
    /// Event published when a strategic pivot is initiated
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.BusinessModelManagement.StrategicPivotInitiated")]
    public class StrategicPivotInitiatedEto : BusinessModelManagementEtoBase
    {
        public PivotType Type { get; set; }
        public string Reason { get; set; }
        public string Description { get; set; }
        public Guid InitiatedBy { get; set; }
        public DateTime TargetCompletionDate { get; set; }
    }
}
