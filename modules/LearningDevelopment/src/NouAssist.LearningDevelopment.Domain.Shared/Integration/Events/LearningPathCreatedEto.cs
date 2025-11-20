using System;
using NouAssist.LearningDevelopment.Integration.Enums;

namespace NouAssist.LearningDevelopment.Integration.Events
{
    [Serializable]
    public class LearningPathCreatedEto
    {
        public Guid LearningPathId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid TopicId { get; set; }
        public Guid SkillId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
