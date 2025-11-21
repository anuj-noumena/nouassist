using System;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Events
{
    /// <summary>
    /// Event raised when a learning resource is completed by a user
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.KnowledgeInnovation.LearningResourceCompleted")]
    public class LearningResourceCompletedEto
    {
        public Guid ResourceId { get; set; }
        public string ResourceTitle { get; set; }
        public Guid UserId { get; set; }
        public DateTime CompletionTime { get; set; }
        public Guid? TenantId { get; set; }
    }
}
