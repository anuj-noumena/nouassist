using System;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Events
{
    /// <summary>
    /// Event published when a hypothesis is validated or invalidated
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.BusinessModelManagement.HypothesisValidated")]
    public class HypothesisValidatedEto : BusinessModelManagementEtoBase
    {
        public Guid CanvasId { get; set; }
        public Guid HypothesisId { get; set; }
        public CanvasComponentType ComponentType { get; set; }
        public ValidationStatus Status { get; set; }
        public string TestResult { get; set; }
    }
}
