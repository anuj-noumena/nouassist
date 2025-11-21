using System;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Enums;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Events
{
    /// <summary>
    /// Event published when a customer interview is created
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.CustomerInterviewCreated")]
    public class CustomerInterviewCreatedEto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public InterviewType Type { get; set; }
        public DateTime ScheduledDate { get; set; }
        public Guid? TenantId { get; set; }
    }
}
