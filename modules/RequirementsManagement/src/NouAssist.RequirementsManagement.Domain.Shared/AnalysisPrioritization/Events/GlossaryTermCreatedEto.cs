using System;
using Volo.Abp.EventBus;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Events
{
    /// <summary>
    /// Event published when a glossary term is created
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.AnalysisPrioritization.GlossaryTermCreated")]
    public class GlossaryTermCreatedEto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Definition { get; set; } = string.Empty;
        public Guid? TenantId { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
