using System;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Enums;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Events
{
    /// <summary>
    /// Event published when a customer interview is completed
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.CustomerInterviewCompleted")]
    public class CustomerInterviewCompletedEto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public InterviewType Type { get; set; }
        public DateTime CompletedDate { get; set; }
        public int InsightCount { get; set; }
        public Guid? TenantId { get; set; }
    }
}
