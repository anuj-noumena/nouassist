using System;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Events
{
    /// <summary>
    /// Event published when a problem hypothesis is validated or invalidated
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.ProblemHypothesisValidated")]
    public class ProblemHypothesisValidatedEto
    {
        public Guid Id { get; set; }
        public string HypothesisStatement { get; set; }
        public ValidationStatus Status { get; set; }
        public DateTime ValidatedDate { get; set; }
        public Guid? TenantId { get; set; }
    }
}
