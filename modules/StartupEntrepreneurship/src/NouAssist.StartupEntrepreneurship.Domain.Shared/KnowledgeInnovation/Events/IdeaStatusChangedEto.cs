using System;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Events
{
    /// <summary>
    /// Event raised when an idea's status changes
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.KnowledgeInnovation.IdeaStatusChanged")]
    public class IdeaStatusChangedEto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public IdeaStatus OldStatus { get; set; }
        public IdeaStatus NewStatus { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime StatusChangeTime { get; set; }
        public Guid StatusChangedById { get; set; }
    }
}
