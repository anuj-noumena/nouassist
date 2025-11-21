using System;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Events
{
    /// <summary>
    /// Event published when a strategic pivot is completed
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.BusinessModelManagement.StrategicPivotCompleted")]
    public class StrategicPivotCompletedEto : BusinessModelManagementEtoBase
    {
        public PivotType Type { get; set; }
        public PivotStatus Status { get; set; }
        public string ActualOutcome { get; set; }
        public DateTime CompletedDate { get; set; }
        public bool WasSuccessful { get; set; }
    }
}
