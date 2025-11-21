using System;
using Volo.Abp.EventBus;

namespace NouAssist.RequirementsManagement.Specification.Events
{
    /// <summary>
    /// Event published when a specification structure is created
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.Specification.SpecificationStructureCreated")]
    public class SpecificationStructureCreatedEto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DocumentType { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime EventTime { get; set; } = DateTime.UtcNow;
    }
}
