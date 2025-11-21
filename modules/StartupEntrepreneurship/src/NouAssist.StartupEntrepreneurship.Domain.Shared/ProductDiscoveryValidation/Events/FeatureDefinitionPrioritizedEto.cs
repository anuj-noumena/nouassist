using System;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Enums;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Events
{
    /// <summary>
    /// Event published when a feature definition is prioritized
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.FeatureDefinitionPrioritized")]
    public class FeatureDefinitionPrioritizedEto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public FeaturePriority Priority { get; set; }
        public MvpStatus MvpStatus { get; set; }
        public DateTime PrioritizedDate { get; set; }
        public Guid? TenantId { get; set; }
    }
}
