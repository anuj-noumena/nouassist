using System;
using System.Collections.Generic;
using NouAssist.RequirementsManagement.ValidationVerification.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.RequirementsManagement.ValidationVerification.Events
{
    /// <summary>
    /// Event published when a review cycle is started
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.ValidationVerification.ReviewCycleStarted")]
    public class ReviewCycleStartedEto : ValidationVerificationEtoBase
    {
        public string Name { get; set; }
        public Guid RequirementId { get; set; }
        public string WorkflowName { get; set; }
        public List<Guid> ReviewerIds { get; set; } = new();
        public DateTime? TargetCompletionDate { get; set; }
    }
}
