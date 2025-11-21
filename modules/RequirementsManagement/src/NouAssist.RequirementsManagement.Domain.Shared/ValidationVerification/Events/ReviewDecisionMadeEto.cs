using System;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;
using NouAssist.RequirementsManagement.ValidationVerification.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.RequirementsManagement.ValidationVerification.Events
{
    /// <summary>
    /// Event published when a review decision is made
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.ValidationVerification.ReviewDecisionMade")]
    public class ReviewDecisionMadeEto : ValidationVerificationEtoBase
    {
        public Guid ReviewCycleId { get; set; }
        public Guid RequirementId { get; set; }
        public Guid ReviewerId { get; set; }
        public ReviewDecisionType DecisionType { get; set; }
        public string Reason { get; set; }
        public bool IsAllReviewsCompleted { get; set; }
    }
}
