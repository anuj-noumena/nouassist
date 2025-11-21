using System;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Enums;
using Volo.Abp.EventBus;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Events
{
    /// <summary>
    /// Event published when a prioritization activity is completed
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.AnalysisPrioritization.PrioritizationActivityCompleted")]
    public class PrioritizationActivityCompletedEto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public PrioritizationMethod Method { get; set; }
        public int ParticipantCount { get; set; }
        public int RequirementCount { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime CompletionTime { get; set; }
    }
}
